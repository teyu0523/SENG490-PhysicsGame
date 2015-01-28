from django.conf import settings
from django.contrib.auth import get_user_model
from django.contrib.auth.models import User
from django.db import models
from django.db.models.signals import post_save
from django.dispatch import receiver

from rest_framework.authtoken.models import Token

# Used for the admin page, to separate users into two groups
class Student(User):
    class Meta:
        proxy = True
        app_label = 'auth'
        verbose_name = 'Student account'
        verbose_name_plural = 'Student accounts'

# Used for the admin page, to separate users into two groups
class Instructor(User):
    class Meta:
        proxy = True
        app_label = 'auth'
        verbose_name = 'Instructor account'
        verbose_name_plural = 'Instructor accounts'

# Used for the admin page, to separate users into two groups
class Admin(User):
    class Meta:
        proxy = True
        app_label = 'auth'
        verbose_name = 'Admin account'
        verbose_name_plural = 'Admin accounts'

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


@receiver(post_save, sender=settings.AUTH_USER_MODEL)
def create_auth_token(sender, instance=None, created=False, **kwargs):
    if created:
        Token.objects.create(user=instance)
