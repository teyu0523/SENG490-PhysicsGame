from rest_framework import serializers

from django.contrib.auth.models import User
import game.models


# Used to serialize a user as simply a username
class UserSerializerLight(serializers.ModelSerializer):
    class Meta:
        model = User
        fields = ('username',)


# Used to serialize a lesson without viewing any question data.
class LessonSerializerLight(serializers.ModelSerializer):
    author = UserSerializerLight(many=False)
    lesson_type = serializers.CharField(source='get_lesson_type_display')

    class Meta:
        model = game.models.Lesson
        fields = ('id', 'lesson_type', 'author', 'topic')


# Used to serialize a class as a list of lessions, primarily for student viewing
class LessonListSerializer(serializers.ModelSerializer):
    instructor = UserSerializerLight(many=False)
    lessons = LessonSerializerLight(many=True)

    class Meta:
        model = game.models.Course
        fields = ('id', 'number', 'name', 'year', 'instructor', 'lessons')
