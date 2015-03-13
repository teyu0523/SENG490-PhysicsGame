from sys import float_info, maxint

from django.conf import settings
from django.contrib.auth.models import User
from django.db import models
from django.db.models.signals import post_save, pre_save, pre_delete, m2m_changed
from django.dispatch import receiver

from rest_framework.authtoken.models import Token

# ========================================================== #
# ==============                            ================ #
# ==============      User Abstractions     ================ #
# ==============                            ================ #
# ========================================================== #


# Used for the admin page, to separate users into two groups
class Student(User):
    def __init__(self, *args, **kwargs):
        self._meta.get_field('is_staff').default = False
        self._meta.get_field('is_superuser').default = False
        super(User, self).__init__(*args, **kwargs)

    class Meta:
        proxy = True
        app_label = 'auth'
        verbose_name = 'Student account'
        verbose_name_plural = 'Student accounts'


# Used for the admin page, to separate users into two groups
class Instructor(User):
    def __init__(self, *args, **kwargs):
        self._meta.get_field('is_staff').default = True
        self._meta.get_field('is_superuser').default = False
        super(User, self).__init__(*args, **kwargs)

    class Meta:
        proxy = True
        app_label = 'auth'
        verbose_name = 'Instructor account'
        verbose_name_plural = 'Instructor accounts'


# Used for the admin page, to separate users into two groups
class Admin(User):
    def __init__(self, *args, **kwargs):
        self._meta.get_field('is_staff').default = True
        self._meta.get_field('is_superuser').default = True
        super(User, self).__init__(*args, **kwargs)

    class Meta:
        proxy = True
        app_label = 'auth'
        verbose_name = 'Admin account'
        verbose_name_plural = 'Admin accounts'


@receiver(post_save, sender=Student)
@receiver(post_save, sender=Instructor)
@receiver(post_save, sender=Admin)
@receiver(post_save, sender=settings.AUTH_USER_MODEL)  # Just in case catchall.
def create_auth_token(sender, instance=None, created=False, **kwargs):
    if created:
        Token.objects.create(user=instance)


# ========================================================== #
# ==============                            ================ #
# ==============      Application Data      ================ #
# ==============                            ================ #
# ========================================================== #

class Lesson(models.Model):
    QUIZ = 'QUI'
    TEST = 'TST'
    ASSIGNMENT = 'ASG'
    PRACTICE = 'PRC'
    LESSON_TYPES = (
        (QUIZ, 'Quiz'),
        (TEST, 'Test'),
        (ASSIGNMENT, 'Assignment'),
        (PRACTICE, 'Practice'),
    )

    lesson_type = models.CharField(max_length=3, choices=LESSON_TYPES, default=ASSIGNMENT)
    author = models.ForeignKey(User, related_name='authored_lessons')
    topic = models.CharField(max_length=256)
    retakes = models.BooleanField(default=True)
    one_sitting = models.BooleanField(default=False)

    def get_total_marks(self):
        aggregates = self.included_questions.all().aggregate(total_marks=models.Sum('marks'))
        return aggregates['total_marks']

    def __str__(self):
        return ("%s - %s" % (self.get_lesson_type_display(), self.topic))

    def get_lesson_type_display(self):
        for choice in self.LESSON_TYPES:
            if choice[0] == self.lesson_type:
                return choice[1]


class Course(models.Model):
    instructor = models.ForeignKey(User, related_name='instructed_courses', blank=True)
    students = models.ManyToManyField(User, related_name='registered_courses', blank=True)
    lessons = models.ManyToManyField(Lesson, related_name='courses', blank=True, through='WeightedLesson')
    number = models.IntegerField()
    name = models.CharField(max_length=256)
    year = models.IntegerField()

    def __str__(self):
        return ("%s - %s" % (self.number, self.name))


class WeightedLesson(models.Model):
    course = models.ForeignKey(Course)
    lesson = models.ForeignKey(Lesson)
    weight = models.FloatField(default=10.0)

    def __str__(self):
        return str(self.lesson)


@receiver(post_save, sender=WeightedLesson)
def post_save_course_relation(sender, instance=None, created=False, **kwargs):
    # Creating grades when a lesson is added to a course
    if created:
        student_grades = instance.course.grades.values_list('id', flat=True)
        LessonGrade.objects.bulk_create([LessonGrade(lesson_id=instance.lesson_id, course_grade_id=grade_id) for grade_id in student_grades])


@receiver(pre_delete, sender=WeightedLesson)
def pre_delete_course_relation(sender, instance=None, using=None, **kwargs):
    # Removing grades when a lesson is removed from a course
    instance.course.grades.all().delete()


@receiver(m2m_changed, sender=Course.students.through)
def students_changed(sender, **kwargs):
    # Storing the old set to compare to the new set to find differences.
    if kwargs["action"] == "pre_clear":
        kwargs["instance"].pre_clear_pk_set = set(kwargs["instance"].students.values_list('id', flat=True))

    # Student(s) added/removed from a course
    # Need to create lesson grades for each student added
    # Need to remove lesson grades for each student removed
    else:
        if kwargs["action"] == "post_add":
            removed = kwargs["instance"].pre_clear_pk_set - kwargs["pk_set"]
            if removed:
                Grade.objects.filter(student_id__in=removed).delete()

            added = kwargs["pk_set"] - kwargs["instance"].pre_clear_pk_set
            if added:
                Grade.objects.bulk_create([Grade(student_id=student_id, course=kwargs["instance"]) for student_id in added])

                course_lessons = set(kwargs["instance"].lessons.values_list('id', flat=True))
                student_grades = set(kwargs["instance"].grades.filter(student_id__in=added).values_list('id', flat=True))
                LessonGrade.objects.bulk_create([LessonGrade(lesson_id=lesson_id, course_grade_id=grade_id) for grade_id in student_grades for lesson_id in course_lessons])


class Question(models.Model):
    NUMERIC = 'NUM'
    CANNONS = 'CAN'
    QUESTION_TYPES = (
        (NUMERIC, "Numeric"),
        (CANNONS, "Cannons"),
    )
    name = models.CharField(max_length=128, default="")
    question_type = models.CharField(max_length=3, choices=QUESTION_TYPES, default=CANNONS)
    lesson = models.ForeignKey(Lesson, related_name='included_questions')
    order = models.IntegerField(default=0)
    marks = models.IntegerField(default=0)
    max_tries = models.IntegerField(default=10)
    playable = models.BooleanField(default=True)

    def get_question_type_display(self):
        for choice in self.QUESTION_TYPES:
            if choice[0] == self.question_type:
                return choice[1]

    def __str__(self):
        if self.name != "":
            return "%s - %s" % (self.lesson, self.name)
        return "%s - %s" % (self.lesson, str(self.order+1))


@receiver(pre_save, sender=Question)
def pre_save_question(sender, instance=None, **kwargs):
    try:
        old_instance = Question.objects.get(pk=instance.pk)

        # Remove the old question extension entry
        if old_instance.question_type != instance.question_type:
            old_instance.answers.all().delete()
            IntegerValue.objects.get(question__pk=old_instance.id).delete()
            FloatingPointValue.objects.get(question__pk=old_instance.id).delete()
            StringValue.objects.get(question__pk=old_instance.id).delete()
            ParagraphValue.objects.get(question__pk=old_instance.id).delete()

        # Handle change in owning lesson
        if old_instance.lesson_id != instance.lesson_id:
            instance.answers.all().delete()

        # Update answers
        if old_instance.marks != instance.marks or old_instance.max_tries != instance.max_tries:
            for answer in instance.answers.all():
                answer.save()

    except Question.DoesNotExist:
        pass


@receiver(post_save, sender=Question)
def post_save_question(sender, instance=None, created=False, **kwargs):
    if not created:
        # cancel if an instance already exists
        if IntegerValue.objects.filter(question__pk=instance.pk).exists() or\
                FloatingPointValue.objects.filter(question__pk=instance.pk).exists() or\
                StringValue.objects.filter(question__pk=instance.pk).exists() or\
                ParagraphValue.objects.filter(question__pk=instance.pk).exists():
            return
        # elif:

    # Create an extension instance for this question.
    if instance.question_type == Question.NUMERIC:
        IntegerValue.objects.create(question=instance, name="expected_answer", order=3, menu=False, editable=False)
        ParagraphValue.objects.create(question=instance, name="question_text", order=0, editable=False)
        ParagraphValue.objects.create(question=instance, name="question_text_mobile", order=1, editable=False)
        StringValue.objects.create(question=instance, name="question_hint", order=2, menu=False, editable=False)
    elif instance.question_type == Question.CANNONS:
        FloatingPointValue.objects.create(question=instance, name="player_pos_x", order=0)
        FloatingPointValue.objects.create(question=instance, name="player_pos_y", order=1)
        FloatingPointValue.objects.create(question=instance, name="player_angle", order=2)
        FloatingPointValue.objects.create(question=instance, name="player_velocity", order=3)
        FloatingPointValue.objects.create(question=instance, name="target_pos_x", order=4)
        FloatingPointValue.objects.create(question=instance, name="target_pos_y", order=5)
    # elif:


class IntegerValue(models.Model):
    question = models.ForeignKey(Question, related_name='integer_values')
    name = models.CharField(null=False, max_length=32)
    order = models.IntegerField(default=0)
    value = models.IntegerField(default=0, null=True, blank=True)
    min_value = models.IntegerField(default=-1000)
    max_value = models.IntegerField(default=1000)
    menu = models.BooleanField(default=True)
    editable = models.BooleanField(default=True)

    class Meta:
        unique_together = (("question", "name"),)

    def __str__(self):
        return self.name


class FloatingPointValue(models.Model):
    question = models.ForeignKey(Question, related_name='floating_point_values')
    name = models.CharField(null=False, max_length=32)
    order = models.IntegerField(default=0)
    value = models.FloatField(default=0, null=True, blank=True)
    min_value = models.FloatField(default=-1)
    max_value = models.FloatField(default=1)
    menu = models.BooleanField(default=True)
    editable = models.BooleanField(default=True)

    class Meta:
        unique_together = (("question", "name"),)

    def __str__(self):
        return self.name


class StringValue(models.Model):
    question = models.ForeignKey(Question, related_name='string_values')
    name = models.CharField(null=False, max_length=32)
    order = models.IntegerField(default=0)
    value = models.CharField(max_length=256, blank=True)
    max_length = models.IntegerField(default=256)
    menu = models.BooleanField(default=False)
    editable = models.BooleanField(default=True)

    class Meta:
        unique_together = (("question", "name"),)

    def __str__(self):
        return self.name


class ParagraphValue(models.Model):
    question = models.ForeignKey(Question, related_name='paragraph_values')
    name = models.CharField(null=False, max_length=32)
    order = models.IntegerField(default=0)
    value = models.CharField(max_length=4096, blank=True)
    max_length = models.IntegerField(default=2096)
    menu = models.BooleanField(default=False)
    editable = models.BooleanField(default=True)

    class Meta:
        unique_together = (("question", "name"),)

    def __str__(self):
        return self.name


class Grade(models.Model):
    student = models.ForeignKey(User, related_name='grades')
    course = models.ForeignKey(Course, related_name='grades')

    def get_final_grade(self):
        final_grade = 0
        for lesson_grade in self.lesson_grades.all():
            aggregates = lesson_grade.get_grades()
            if(aggregates['grade_max'] > 0):
                final_grade += (aggregates['grade']/aggregates['grade_max'])
        return final_grade

    def __str__(self):
        return "%s (%d) - %s" % (self.course.name, self.course.year, self.student.username)


class LessonGrade(models.Model):
    NOTSTARTED = 'NOT'
    STARTED = 'SRT'
    FINISHED = 'FIN'
    LESSON_STATES = (
        (NOTSTARTED, 'Not Started'),
        (STARTED, 'Started'),
        (FINISHED, 'Finished'),
    )

    lesson = models.ForeignKey(Lesson, related_name='student_results')
    course_grade = models.ForeignKey(Grade, related_name='lesson_grades')
    lesson_state = models.CharField(max_length=3, choices=LESSON_STATES, default=NOTSTARTED)

    def __str__(self):
        return "%s - %s" % (self.lesson, self.course_grade.student.username)

    def get_lesson_state_display(self):
        for choice in self.LESSON_STATES:
            if choice[0] == self.lesson_state:
                return choice[1]

    def get_grades(self):
        # Doesn't require question grades to exist!
        # Also lazy loads!
        if not hasattr(self, 'aggregates'):
            self.aggregates = {}
            self.aggregates['answered_questions'] = self.question_results.count()
            self.aggregates['total_questions'] = self.lesson.included_questions.count()
            self.aggregates['grade_max'] = self.lesson.get_total_marks()
            if self.aggregates['grade_max'] == None:
                self.aggregates['grade_max'] = 0
            self.aggregates['grade'] = self.question_results.all().aggregate(grade=models.Sum('grade'))['grade']
            if self.aggregates['grade'] == None:
                self.aggregates['grade'] = 0
        return self.aggregates


class Answer(models.Model):
    question = models.ForeignKey(Question, related_name='answers')
    lesson_grade = models.ForeignKey(LessonGrade, related_name='question_results')
    total_tries = models.IntegerField(default=0)
    grade = models.FloatField(default=0)

    def save(self, *args, **kwargs):
        self.calculate_grade()
        super(Answer, self).save(*args, **kwargs)

    def calculate_grade(self):
        grade_percent = 0.5 + (float(self.question.max_tries - self.total_tries + 1) / self.question.max_tries)/2
        if self.total_tries > self.question.max_tries:
            grade_percent = 0
        self.grade = grade_percent * self.question.marks

    def __str__(self):
        return "%s - %s" % (self.question, self.lesson_grade.course_grade.student.username)

@receiver(post_save, sender=Answer)
def post_save_answer(sender, instance=None, created=False, **kwargs):
    if created and instance is not None:
        if instance.question.question_type == Question.NUMERIC:
            IntegerAnswer.objects.create(answer=instance, name="submitted_answer")
        elif instance.question.question_type == Question.CANNONS:
            FloatingPointAnswer.objects.create(answer=instance, name="player_pos_x")
            FloatingPointAnswer.objects.create(answer=instance, name="player_pos_y")
            FloatingPointAnswer.objects.create(answer=instance, name="player_angle")
            FloatingPointAnswer.objects.create(answer=instance, name="player_velocity")
            FloatingPointAnswer.objects.create(answer=instance, name="target_pos_x")
            FloatingPointAnswer.objects.create(answer=instance, name="target_pos_y")


class IntegerAnswer(models.Model):
    answer = models.ForeignKey(Answer, related_name='integer_answers')
    name = models.CharField(null=False, max_length=32)
    value = models.IntegerField(default=0, null=False, blank=False)
    submitted = models.BooleanField(default=False)

    class Meta:
        unique_together = (("answer", "name"),)

    def __str__(self):
        return self.name


class FloatingPointAnswer(models.Model):
    answer = models.ForeignKey(Answer, related_name='floating_point_answers')
    name = models.CharField(null=False, max_length=32)
    value = models.FloatField(default=0, null=False, blank=False)
    submitted = models.BooleanField(default=False)

    class Meta:
        unique_together = (("answer", "name"),)

    def __str__(self):
        return self.name


class StringAnswer(models.Model):
    answer = models.ForeignKey(Answer, related_name='string_answers')
    name = models.CharField(null=False, max_length=32)
    value = models.CharField(max_length=256, blank=True)
    submitted = models.BooleanField(default=False)

    class Meta:
        unique_together = (("answer", "name"),)

    def __str__(self):
        return self.name


class ParagraphAnswer(models.Model):
    answer = models.ForeignKey(Answer, related_name='paragraph_answers')
    name = models.CharField(null=False, max_length=32)
    value = models.CharField(max_length=4096, blank=True)
    submitted = models.BooleanField(default=False)

    class Meta:
        unique_together = (("answer", "name"),)

    def __str__(self):
        return self.name
