from django.contrib import admin
from game.models import Student, Instructor, Admin
from game.models import Course, Lesson, WeightedLesson, Question, IntegerValue, FloatingPointValue, StringValue, ParagraphValue
from game.models import Grade, LessonGrade, Answer, IntegerAnswer, FloatingPointAnswer, StringAnswer, ParagraphAnswer
from django.contrib.auth.models import User, Group
from django.contrib.auth.admin import UserAdmin

# ========================================================== #
# ==============                            ================ #
# ==============       Custom Classes       ================ #
# ==============                            ================ #
# ========================================================== #


class InstructorListFilter(admin.SimpleListFilter):
    title = 'instructor'
    parameter_name = 'instructor'

    def lookups(self, request, model_admin):
        instructors = User.objects.filter(is_staff=True)
        return ((instructor.id, instructor.username) for instructor in instructors)

    def queryset(self, request, queryset):
        if self.value() != None:
            return queryset.filter(instructor__pk=self.value())
        else:
            return queryset


class AuthorListFilter(InstructorListFilter):
    title = 'author'
    parameter_name = 'author'

    def queryset(self, request, queryset):
        if self.value() != None:
            return queryset.filter(author__pk=self.value())
        else:
            return queryset


class ReadOnlyTabularInline(admin.TabularInline):
    def __init__(self, *args, **kwargs):
        super(ReadOnlyTabularInline, self).__init__(*args, **kwargs)
        self.readonly_fields = self.model._meta.get_all_field_names()

    def has_add_permission(self, request, obj=None):
        return False

    def has_delete_permission(self, request, obj=None):
        return False


class EditOnlyTabularInline(admin.TabularInline):
    def has_add_permission(self, request, obj=None):
        return False

    def has_delete_permission(self, request, obj=None):
        return False


def custom_titled_filter(title):
    class Wrapper(admin.FieldListFilter):
        def __new__(cls, *args, **kwargs):
            instance = admin.FieldListFilter.create(*args, **kwargs)
            instance.title = title
            return instance
    return Wrapper


def custom_titled_relation_filter(title):
    class Wrapper(admin.RelatedFieldListFilter):
        def __new__(cls, *args, **kwargs):
            instance = admin.FieldListFilter.create(*args, **kwargs)
            instance.title = title
            return instance
    return Wrapper

# ========================================================== #
# ==============                            ================ #
# ==============         User Admin         ================ #
# ==============                            ================ #
# ========================================================== #

admin.site.unregister(User)


@admin.register(Admin)
class SystemAdmin(UserAdmin):
    # Forces group relations based on user state.
    def update_groups(self, new_object):
        g = Group.objects.get(name='instructor')
        if new_object.is_staff:
            g.user_set.add(new_object)
        else:
            g.user_set.remove(new_object)

    # Overridden to automatically give instructors proper permissions set
    def save_related(self, request, form, formsets, change):
        super(UserAdmin, self).save_related(request, form, formsets, change)
        self.update_groups(form.instance)

    # Overridden to redirect to different lists when user type is changed
    def response_change(self, request, obj):
        result = super(UserAdmin, self).response_change(request, obj)
        if "_continue" in request.POST:
            redirect = result['Location'].split("/")
            if(obj.is_superuser and obj.is_staff):
                redirect[3] = "admin"
            elif(not obj.is_superuser and obj.is_staff):
                redirect[3] = "instructor"
            else:
                redirect[3] = "student"
            redirect = "/".join(redirect)
            result['Location'] = redirect

        return result

    # Overridden to limit results to correct users
    def get_queryset(self, request):
        qs = super(UserAdmin, self).get_queryset(request)
        qs = qs.filter(is_superuser=True)
        return qs


@admin.register(Instructor)
class InstructorAdmin(SystemAdmin):
    # Overridden to limit results to correct users
    def get_queryset(self, request):
        qs = super(UserAdmin, self).get_queryset(request)
        qs = qs.filter(is_staff=True, is_superuser=False)
        return qs


@admin.register(Student)
class StudentAdmin(SystemAdmin):
    # Overridden to limit results to correct users
    def get_queryset(self, request):
        qs = super(UserAdmin, self).get_queryset(request)
        qs = qs.filter(is_staff=False, is_superuser=False)
        return qs

# ========================================================== #
# ==============                            ================ #
# ==============      Questions Admin       ================ #
# ==============                            ================ #
# ========================================================== #


class WeightedLessonInline(admin.StackedInline):
    model = WeightedLesson
    extra = 1


@admin.register(Course)
class CourseAdmin(admin.ModelAdmin):
    fieldsets = (
        (None, {
            'fields': ('number', 'name', 'year', 'instructor', 'description',)
        }),
        ('Students', {
            'fields': ('students',)
        }),
    )
    filter_horizontal = ('students',)
    list_display = ('course_details', 'year',)
    list_filter = (InstructorListFilter, 'year')
    inlines = (WeightedLessonInline, )

    def course_details(self, obj):
        return obj.__str__().upper()
    course_details.short_description = 'Course'

    # Overridden to limit results to correct users
    def formfield_for_foreignkey(self, db_field, request, **kwargs):
        if db_field.name == "instructor":
            kwargs["queryset"] = User.objects.filter(is_staff=True, is_superuser=False)
        return super(CourseAdmin, self).formfield_for_foreignkey(db_field, request, **kwargs)

    # Overridden to limit results to correct users
    def formfield_for_manytomany(self, db_field, request, **kwargs):
        if db_field.name == "students":
            kwargs["queryset"] = User.objects.filter(is_staff=False, is_superuser=False)
        return super(CourseAdmin, self).formfield_for_manytomany(db_field, request, **kwargs)


@admin.register(Lesson)
class LessonAdmin(admin.ModelAdmin):
    list_display = ('lesson_details', 'author',)
    list_filter = (
        AuthorListFilter,
    )
    readonly_fields = ('total_marks', )

    def lesson_details(self, obj):
        return obj.__str__()
    lesson_details.short_description = 'Lesson'

    def total_marks(self, instance):
        return instance.get_total_marks()
    total_marks.integer = True

    # Overridden to limit results to correct users
    def formfield_for_foreignkey(self, db_field, request, **kwargs):
        if db_field.name == "author":
            kwargs["queryset"] = User.objects.filter(is_staff=True, is_superuser=False)
        return super(LessonAdmin, self).formfield_for_foreignkey(db_field, request, **kwargs)


class IntegerValueInline(EditOnlyTabularInline):
    model = IntegerValue
    readonly_fields = ('name', 'menu', 'order')


class FloatingPointValueInline(EditOnlyTabularInline):
    model = FloatingPointValue
    readonly_fields = ('name', 'menu', 'order')


class StringValueInline(EditOnlyTabularInline):
    model = StringValue
    readonly_fields = ('name', 'menu', 'order')


class ParagraphValueInline(EditOnlyTabularInline):
    model = ParagraphValue
    readonly_fields = ('name', 'menu', 'order')


@admin.register(Question)
class QuestionAdmin(admin.ModelAdmin):
    list_display = ('question_details', 'question_type',)
    list_filter = (
        'lesson__topic',
        ('lesson__courses__name', custom_titled_relation_filter("name")),
        'question_type',
    )
    inlines = (IntegerValueInline, FloatingPointValueInline, StringValueInline, ParagraphValueInline)

    def question_details(self, obj):
        return obj.__str__()
    question_details.short_description = 'Question'


# ========================================================== #
# ==============                            ================ #
# ==============        Grades Admin        ================ #
# ==============                            ================ #
# ========================================================== #

class LessonGradeInline(admin.TabularInline):
    model = LessonGrade
    fields = ('lesson', 'lesson_state', 'course_grade', 'grade', 'grade_max', 'answered_questions', 'weight',)
    readonly_fields = ('lesson', 'lesson_state', 'course_grade', 'grade', 'grade_max', 'answered_questions', 'weight',)
    min_num = 0
    extra = 0

    def has_add_permission(self, request, obj=None):
        return False

    def has_delete_permission(self, request, obj=None):
        return False

    def lesson_state(self, instance):
        return instance.get_lesson_state_display(instance.lesson_state)
    lesson_state.string = True

    def grade(self, instance):
        aggregates = instance.get_grades()
        if aggregates['grade'] > 0:
            return "%.2f" % (aggregates['grade'])
        else:
            return "N/A"
    grade.string = True

    def grade_max(self, instance):
        if instance.id is not None:
            aggregates = instance.get_grades()
            return aggregates['grade_max']
        return 0
    grade_max.integer = True

    def weight(self, instance):
        return WeightedLesson.objects.get(lesson_id=instance.lesson_id, course_id=instance.course_grade.course_id).weight
    weight.decimal = True
    weight.decimal_places = 2

    def answered_questions(self, instance):
        if instance.id is not None:
            aggregates = instance.get_grades()
            return "%d/%d" % (aggregates['answered_questions'], aggregates['total_questions'])
        return "---"
    answered_questions.string = True


@admin.register(Grade)
class GradeAdmin(admin.ModelAdmin):
    list_display = ('course', 'student')
    list_filter = (('course__name', custom_titled_relation_filter("course")),)
    readonly_fields = ('student', 'course', 'final_grade')

    inlines = (LessonGradeInline,)

    def has_add_permission(self, request, obj=None):
        return False

    def has_delete_permission(self, request, obj=None):
        return False

    def final_grade(self, instance):
        return "%g%%" % (round(instance.get_final_grade(), 2))
    final_grade.string = True


class IntegerAnswerInline(ReadOnlyTabularInline):
    model = IntegerAnswer
    readonly_fields = ('name',)


class FloatingPointAnswerInline(ReadOnlyTabularInline):
    model = FloatingPointAnswer
    readonly_fields = ('name',)


class StringAnswerInline(ReadOnlyTabularInline):
    model = StringAnswer
    readonly_fields = ('name',)


class ParagraphAnswerInline(ReadOnlyTabularInline):
    model = ParagraphAnswer
    readonly_fields = ('name',)


@admin.register(Answer)
class AnswerAdmin(admin.ModelAdmin):
    list_display = ('question', 'lesson_grade', 'total_tries', 'grade_percent',)
    readonly_fields = ('question', 'lesson_grade', 'grade', 'grade_max', 'grade_percent',)
    inlines = (IntegerAnswerInline, FloatingPointAnswerInline, StringAnswerInline, ParagraphAnswerInline)

    def has_add_permission(self, request, obj=None):
        return False

    def has_delete_permission(self, request, obj=None):
        return False

    def grade_percent(self, instance):
        return "%g%%" % (round(instance.grade / instance.question.marks * 100, 2))
    grade_percent.string = True

    def grade_max(self, instance):
        return instance.question.marks
    grade_max.integer = True
