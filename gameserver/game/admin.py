from django.contrib import admin
from game.models import *
from django.contrib.auth.models import User
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
		if(self.value() != None) :
			return queryset.filter(instructor__pk=self.value())
		else:
			return queryset

class AuthorListFilter(InstructorListFilter):
	title = 'author'
	parameter_name = 'author'
	def queryset(self, request, queryset):
		if(self.value() != None) :
			return queryset.filter(author__pk=self.value())
		else:
			return queryset

class ReadOnlyStackedInline(admin.StackedInline):
	def __init__(self, *args, **kwargs):
		super(ReadOnlyAdmin, self).__init__(*args, **kwargs)
		readonly_fields = self.model._meta.get_all_field_names()
	
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
		if(new_object.is_staff == True):
			g.user_set.add(new_object)
		else:
			g.user_set.remove(new_object)

	# Overridden to automatically give instructors proper permissions set
	def save_related(self, request, form, formsets, change):
		super(UserAdmin, self).save_related(request, form, formsets, change)
		self.update_groups(form.instance)

	# Overridden to redirect to different lists when user type is changed
	def response_change(self, request, obj):
		result = super(UserAdmin, self).response_change(request, obj);
		if "_continue" in request.POST:
			redirect = result['Location'].split("/")
			if(obj.is_superuser and obj.is_staff):
				redirect[3] = "admin"
			elif(not obj.is_superuser and obj.is_staff):
				redirect[3] = "instructor"
			else:
				redirect[3] = "student"
			redirect = "/".join(redirect)
			result['Location'] = redirect;
		
		return result;

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
# ==============     Application Admin      ================ #
# ==============                            ================ #
# ========================================================== #

@admin.register(Course)
class CourseAdmin(admin.ModelAdmin):
	fieldsets = (
		(None, {
			'fields': ('number', 'name', 'year', 'instructor',)
		}),
		('Students', {
			'classes': ('collapse',),
			'fields': ('students',)
		}),
	)
	filter_horizontal = ('students',)
	list_display = ('course_details', 'year',)
	list_filter = (InstructorListFilter, 'year')

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

	# Limit edits to specific users
	#ModelAdmin.has_add_permission(request)
	#ModelAdmin.has_change_permission(request, obj=None)
	#ModelAdmin.has_delete_permission(request, obj=None)

	# Change query set based on signed in user
	# ModelAdmin.get_queryset(request)

@admin.register(Lesson)
class LessonAdmin(admin.ModelAdmin):
	list_display = ('lesson_details', 'course', 'author',)
	list_filter = (
		('course__name', custom_titled_relation_filter("course")),
		'course__year',
		AuthorListFilter,
	)
	
	def lesson_details(self, obj):
		return obj.__str__()
	lesson_details.short_description = 'Lesson'

	# Overridden to limit results to correct users
	def formfield_for_foreignkey(self, db_field, request, **kwargs):
		if db_field.name == "author":
			kwargs["queryset"] = User.objects.filter(is_staff=True, is_superuser=False)
		return super(LessonAdmin, self).formfield_for_foreignkey(db_field, request, **kwargs)

class CannonsQuestionInline(admin.StackedInline):
	model = CannonsQuestion

	def has_add_permission(self, request, obj=None):
		return False
	def has_delete_permission(self, request, obj=None):
		return False

@admin.register(Question)
class QuestionAdmin(admin.ModelAdmin):
	list_display = ('question_details', 'question_type',)
	list_filter = (
		('lesson__course__name', custom_titled_relation_filter("course")),
		'question_type',
	)
	inlines = ()

	def question_details(self, obj):
		return obj.__str__()
	question_details.short_description = 'Question'	

	def change_view(self, request, object_id, form_url='', extra_context=None):
		instance = Question.objects.get(pk=object_id)
		if instance.question_type == Question.CANNONS:
			self.inlines = (CannonsQuestionInline, )
		return super(QuestionAdmin, self).change_view(request, object_id)

class CannonsAnswerInline(ReadOnlyStackedInline):
	model = CannonsAnswer

@admin.register(Answer)
class AnswerAdmin(admin.ModelAdmin):
	list_display = ('question', 'lesson_results', 'total_tries', 'weighted_mark',)
	readonly_fields = ('question', 'lesson_results', 'weighted_mark')

	def change_view(self, request, object_id, form_url='', extra_context=None):
		instance = Question.objects.get(pk=object_id)
		if instance.question.question_type == Question.CANNONS:
			self.inlines = (CannonsAnswerInline, )
		return super(QuestionAdmin, self).change_view(request, object_id)

	def has_add_permission(self, request, obj=None):
		return False
	def has_delete_permission(self, request, obj=None):
		return False
