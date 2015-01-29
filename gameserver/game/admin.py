from django.contrib import admin
from game.models import *
from django.contrib.auth.models import User
from django.contrib.auth.admin import UserAdmin

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
		

admin.site.unregister(User)

@admin.register(Admin)
class InstructorAdmin(UserAdmin):
	# Override response_change to fix dat 404!
    def get_queryset(self, request):
        qs = super(UserAdmin, self).queryset(request)
        qs = qs.filter(is_superuser=True)
        return qs

@admin.register(Instructor)
class InstructorAdmin(UserAdmin):
    def get_queryset(self, request):
        qs = super(UserAdmin, self).queryset(request)
        qs = qs.filter(is_staff=True, is_superuser=False)
        return qs

@admin.register(Student)
class StudentAdmin(UserAdmin):
    def get_queryset(self, request):
        qs = super(UserAdmin, self).queryset(request)
        qs = qs.filter(is_staff=False, is_superuser=False)
        return qs

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

@admin.register(Lesson)
class LessonAdmin(admin.ModelAdmin):
	list_display = ('lesson_details', 'course', 'author',)
	list_filter = ('course__name', 'course__year', AuthorListFilter,)
	
	def lesson_details(self, obj):
		return obj.__str__()
	lesson_details.short_description = 'Lesson'
