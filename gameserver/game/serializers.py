from rest_framework import serializers

import game.models

# ========================================================== #
# ==============                            ================ #
# ==============       Course Viewing       ================ #
# ==============                            ================ #
# ========================================================== #



# ========================================================== #
# ==============                            ================ #
# ==============       Lesson Viewing       ================ #
# ==============                            ================ #
# ========================================================== #


class IntegerValueSerializer(serializers.ModelSerializer):

    class Meta:
        model = game.models.IntegerValue
        fields = ('id', 'name', 'order', 'value', 'min_value', 'max_value', 'menu', 'editable')


class FloatingPointValueSerializer(serializers.ModelSerializer):

    class Meta:
        model = game.models.FloatingPointValue
        fields = ('id', 'name', 'order', 'value', 'min_value', 'max_value', 'menu', 'editable')


class StringValueSerializer(serializers.ModelSerializer):

    class Meta:
        model = game.models.StringValue
        fields = ('id', 'name', 'order', 'value', 'max_length', 'menu', 'editable')


class ParagraphValueSerializer(serializers.ModelSerializer):

    class Meta:
        model = game.models.ParagraphValue
        fields = ('id', 'name', 'order', 'value', 'max_length', 'menu', 'editable')


class QuestionSerializer(serializers.ModelSerializer):

    class Meta:
        model = game.models.Question
        fields = ('id', 'name', 'order', 'marks', 'max_tries', 'playable')

    def to_representation(self, obj):
        result = super(QuestionSerializer, self).to_representation(obj)
        result['type'] = obj.get_question_type_display()
        return result


class LessonGradeSerializer(serializers.ModelSerializer):
    lesson_state = serializers.ChoiceField(choices=game.models.LessonGrade.LESSON_STATES, default=game.models.LessonGrade.NOTSTARTED)

    class Meta:
        model = game.models.LessonGrade
        fields = ('id', 'lesson_state')


class LessonSerializer(serializers.ModelSerializer):
    author = serializers.SlugRelatedField(read_only=True, slug_field='username')
    lesson_type = serializers.ChoiceField(choices=game.models.Lesson.LESSON_TYPES, default=game.models.Lesson.ASSIGNMENT)

    class Meta:
        model = game.models.Lesson
        fields = ('author', 'lesson_type', 'topic', 'retakes', 'one_sitting')


# ========================================================== #
# ==============                            ================ #
# ==============        Grade Viewing       ================ #
# ==============                            ================ #
# ========================================================== #

class AnswerSerializer(serializers.ModelSerializer):
    question = serializers.PrimaryKeyRelatedField(read_only=True)

    class Meta:
        model = game.models.Answer
        fields = ('id', 'question', 'grade', 'total_tries')


class IntegerAnswerSerializer(serializers.ModelSerializer):

    class Meta:
        model = game.models.IntegerAnswer
        fields = ('id', 'name', 'value', 'submitted')

    def update(self, instance, validated_data):
        instance.value = int(validated_data.get('value', instance.value))
        instance.submitted = True
        return instance

    def validate(self, data):
        return data


class FloatingPointAnswerSerializer(serializers.ModelSerializer):

    class Meta:
        model = game.models.FloatingPointAnswer
        fields = ('id', 'name', 'value', 'submitted')

    def update(self, instance, validated_data):
        instance.value = float(validated_data.get('value', instance.value))
        instance.submitted = True
        return instance

    def validate(self, data):
        return data


class StringAnswerSerializer(serializers.ModelSerializer):

    class Meta:
        model = game.models.StringAnswer
        fields = ('id', 'name', 'value', 'submitted')

    def update(self, instance, validated_data):
        instance.value = validated_data.get('value', instance.value)
        instance.submitted = True
        return instance

    def validate(self, data):
        return data


class ParagraphAnswerSerializer(serializers.ModelSerializer):

    class Meta:
        model = game.models.ParagraphAnswer
        fields = ('id', 'name', 'value', 'submitted')

    def update(self, instance, validated_data):
        instance.value = validated_data.get('value', instance.value)
        instance.submitted = True
        return instance

    def validate(self, data):
        return data
