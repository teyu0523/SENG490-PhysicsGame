from rest_framework import serializers

from django.contrib.auth.models import User
import game.models

# ========================================================== #
# ==============                            ================ #
# ==============        Grade Viewing       ================ #
# ==============                            ================ #
# ========================================================== #


# Used to serialize a user as simply a username
class UserSerializerLight(serializers.ModelSerializer):
    class Meta:
        model = User
        fields = ('username',)


# Used to serialize a course without lesson data
class CourseSerializerLight(serializers.ModelSerializer):
    instructor = UserSerializerLight(many=False)

    class Meta:
        model = game.models.Course
        fields = ('id', 'number', 'name', 'year', 'instructor')


# Used to serialize a lesson without including question data.
class LessonSerializerLight(serializers.ModelSerializer):
    author = UserSerializerLight('username')

    class Meta:
        model = game.models.Course
        fields = ('id', 'lesson_type', 'author', 'topic')


# Used to serialize a lesson grade as a lesson substitute
class LessonGradeSerializerLight(serializers.ModelSerializer):
    lesson = LessonSerializerLight(many=False)

    class Meta:
        model = game.models.LessonGrade
        fields = ('id', 'lesson', 'lesson_state')


# Used to serializer a students grade as a course subsitute
class GradeListSerializer(serializers.ModelSerializer):
    lesson_grades = LessonGradeSerializerLight(many=True)
    course = CourseSerializerLight(many=False)

    class Meta:
        model = game.models.Grade
        fields = ('course', 'lesson_grades')

# ========================================================== #
# ==============                            ================ #
# ==============       Lesson Viewing       ================ #
# ==============                            ================ #
# ========================================================== #


class QuestionSerializer(serializers.ModelSerializer):

    class Meta:
        model = game.models.Question
        fields = ('id', 'name', 'question_type', 'order', 'marks', 'max_tries', 'playable')


class LessonSerializer(serializers.ModelSerializer):
    included_questions = QuestionSerializer(many=True)

    class Meta:
        model = game.models.Lesson
        fields = ('lesson_type', 'topic', 'retakes', 'one_sitting', 'included_questions')


class AnswerSerializer(serializers.ModelSerializer):
    question = serializers.PrimaryKeyRelatedField(read_only=True)

    class Meta:
        model = game.models.Answer
        fields = ('question', 'total_tries', 'grade')


# Used to serialize a lesson grade as a full lesson
class LessonGradeSerializer(serializers.ModelSerializer):
    lesson = LessonSerializerLight(many=False)

    class Meta:
        model = game.models.LessonGrade
        fields = ('id', 'lesson', 'lesson_state', 'question_results')
