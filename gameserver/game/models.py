from django.conf import settings
from django.contrib.auth import get_user_model
from django.contrib.auth.models import User, Group
from django.db import models
from django.db.models.signals import post_save, pre_save
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
@receiver(post_save, sender=settings.AUTH_USER_MODEL) #<< Just in case catchall.
def create_auth_token(sender, instance=None, created=False, **kwargs):
    if created:
        Token.objects.create(user=instance)

# ========================================================== #
# ==============                            ================ #
# ==============      Application Data      ================ #
# ==============                            ================ #
# ========================================================== #

class Course(models.Model):
	instructor = models.ForeignKey(User, related_name='instructed_courses')
	students = models.ManyToManyField(User, related_name='registered_courses')
	number = models.IntegerField()
	name = models.CharField(max_length=256)
	year = models.IntegerField()

	def __str__(self):
		return ("%s - %s" % (self.number, self.name))

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
	course = models.ForeignKey(Course, related_name='lessons')
	author = models.ForeignKey(User, related_name='authored_lessons')
	topic = models.CharField(max_length=256)

	def __str__(self):
		return ("%s - %s" % (self.get_lesson_type_display(), self.topic))

	def get_lesson_type_display(self):
		for choice in self.LESSON_TYPES:
			if choice[0] == self.lesson_type:
				return choice[1]

class LessonResults(models.Model):
	lesson = models.ForeignKey(Lesson, related_name='student_results')
	student = models.ForeignKey(User, related_name='lesson_results')
	grade = models.IntegerField(default=0)
	grade_max = models.IntegerField(default=0)
	percent = models.FloatField(default=0)

class Question(models.Model):
	CANNONS = 'CAN'
	QUESTION_TYPES = (
		(CANNONS, "Cannons"),
	)
	name = models.CharField(max_length=128, default="")
	question_type = models.CharField(max_length=3, choices=QUESTION_TYPES, default=CANNONS)
	lesson = models.ForeignKey(Lesson, related_name='included_questions')
	order = models.IntegerField()
	weight = models.IntegerField()
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
def remove_question_extension(sender, instance=None, **kwargs):
	try:
		old_instance = Question.objects.get(pk=instance.pk)

		# No change necessary
		if old_instance.question_type == instance.question_type:
			return
		# Remove the old question extension entry
		else:
			if old_instance.question_type == Question.CANNONS:
				CannonsQuestion.objects.get(question__pk=old_instance.id).delete()
			#else if
	except Question.DoesNotExist:
		pass

@receiver(post_save, sender=Question)
def add_question_extension(sender, instance=None, created=False, **kwargs):
	if not created:
		# cancel if an instance already exists
		if instance.question_type == Question.CANNONS:
			if CannonsQuestion.objects.filter(question__id=instance.id).exists():
				return
		#elif:

	# Create an extension instance for this question.
	if instance.question_type == Question.CANNONS:
		CannonsQuestion.objects.create(question=instance)
	#elif:

class Answer(models.Model):
	question = models.ForeignKey(Question, related_name='answers')
	lesson_results = models.ForeignKey(Lesson, related_name='answers')
	total_tries = models.IntegerField()
	weighted_mark = models.FloatField()

class CannonsQuestion(models.Model):
	question = models.OneToOneField(Question, related_name='cannons_extension')
	player_tank_pos_x = models.FloatField(null=True, blank=True);
	player_tank_pos_y = models.FloatField(null=True, blank=True);
	player_tank_angle = models.FloatField(null=True, blank=True);
	player_tank_velocity = models.FloatField(null=True, blank=True);
	enemy_tank_pos_x = models.FloatField(null=True, blank=True);
	enemy_tank_pos_y = models.FloatField(null=True, blank=True);
	enemy_tank_angle = models.FloatField(null=True, blank=True);
	enemy_tank_velocity = models.FloatField(null=True, blank=True);

class CannonsAnswer(models.Model):
	answer = models.OneToOneField(Answer, related_name='cannons_extension')
	player_tank_pos_x = models.FloatField(null=True, blank=True);
	player_tank_pos_y = models.FloatField(null=True, blank=True);
	player_tank_angle = models.FloatField(null=True, blank=True);
	player_tank_velocity = models.FloatField(null=True, blank=True);
	enemy_tank_pos_x = models.FloatField(null=True, blank=True);
	enemy_tank_pos_y = models.FloatField(null=True, blank=True);
	enemy_tank_angle = models.FloatField(null=True, blank=True);
	enemy_tank_velocity = models.FloatField(null=True, blank=True);
