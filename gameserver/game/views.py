import sys

from django.http import JsonResponse, HttpResponseBadRequest, HttpResponse

from rest_framework.views import APIView
from rest_framework.authentication import TokenAuthentication
from rest_framework.permissions import IsAuthenticated

# from game.mixins import *
from game.models import Grade, LessonGrade, WeightedLesson, Question, Answer, IntegerAnswer, FloatingPointAnswer, StringAnswer, ParagraphAnswer
import game.serializers


class StudentListLessons(APIView):
    authentication_classes = (TokenAuthentication,)
    permission_classes = (IsAuthenticated,)

    def get(self, request, format=None):
        course_structure = {}

        course_structure['username'] = request.user.username
        courses = []

        query = Grade.objects.select_related('course').select_related('course__instructor')
        query = query.prefetch_related('lesson_grades').select_related('lesson_grades__lesson')

        for grade in query.filter(student__id=self.request.user.id):
            course = {}
            course['id'] = grade.id
            course['course_id'] = grade.course.id
            course['current_grade'] = grade.get_final_grade()
            course['instructor'] = str(grade.course.instructor)
            course['number'] = grade.course.number
            course['name'] = grade.course.name
            course['year'] = grade.course.year
            course['lessons'] = []

            for lesson_grade in grade.lesson_grades.all():
                lesson = {}
                lesson['id'] = lesson_grade.id
                lesson['lesson_id'] = lesson_grade.lesson.id
                lesson['weight'] = WeightedLesson.objects.get(lesson_id=lesson_grade.lesson_id, course_id=grade.course_id).weight
                lesson['lesson_state'] = lesson_grade.get_lesson_state_display()
                lesson['total_questions'] = lesson_grade.get_grades()['total_questions']
                lesson['answered_questions'] = lesson_grade.get_grades()['answered_questions']
                if(lesson_grade.get_grades()['grade_max'] > 0):
                    lesson['grade'] = str(lesson_grade.get_grades()['grade']/lesson_grade.get_grades()['grade_max'])
                else:
                    lesson['grade'] = 'N/A'
                lesson['type'] = lesson_grade.lesson.get_lesson_type_display()
                lesson['name'] = lesson_grade.lesson.topic
                lesson['retakes_allowed'] = lesson_grade.lesson.retakes
                lesson['closable'] = lesson_grade.lesson.one_sitting

                course['lessons'].append(lesson)

            courses.append(course)

        course_structure['courses'] = courses

        return JsonResponse(course_structure)


class StudentLessonDetails(APIView):
    authentication_classes = (TokenAuthentication,)
    permission_classes = (IsAuthenticated,)

    def getLesson(self, request, lesson_id):
        return LessonGrade.objects.get(course_grade__student_id=request.user.id, lesson_id=lesson_id)

    def get(self, request, lesson_id, format=None):
        lesson_grade = self.getLesson(request, lesson_id)
        if lesson_grade is None:
            return HttpResponseBadRequest("Student is not signed up for this lesson!")

        if lesson_grade.lesson_state == LessonGrade.NOTSTARTED:
            lesson_grade.lesson_state = LessonGrade.STARTED
            lesson_grade.save()

        lesson_structure = game.serializers.LessonGradeSerializer(lesson_grade).data
        lesson_structure.update(game.serializers.LessonSerializer(lesson_grade.lesson).data)
        lesson_structure['lesson_id'] = lesson_grade.lesson.id
        lesson_structure['weight'] = WeightedLesson.objects.get(lesson_id=lesson_grade.lesson_id, course_id=lesson_grade.course_grade.course_id).weight
        lesson_structure['total_questions'] = lesson_grade.get_grades()['total_questions']
        lesson_structure['answered_questions'] = lesson_grade.get_grades()['answered_questions']
        if(lesson_grade.get_grades()['grade_max'] > 0):
            lesson_structure['grade'] = str(lesson_grade.get_grades()['grade']/lesson_grade.get_grades()['grade_max']*100)
        else:
            lesson_structure['grade'] = 'N/A'

        questions = []

        for question in lesson_grade.lesson.included_questions.all():
            question_structure = game.serializers.QuestionSerializer(question).data

            values = {}

            for value in question.integer_values.all():
                value_structure = game.serializers.IntegerValueSerializer(value).data
                value_structure['type'] = 'integer'
                values[value_structure['name']] = value_structure
            for value in question.floating_point_values.all():
                value_structure = game.serializers.FloatingPointValueSerializer(value).data
                value_structure['type'] = 'float'
                values[value_structure['name']] = value_structure
            for value in question.string_values.all():
                value_structure = game.serializers.StringValueSerializer(value).data
                value_structure['type'] = 'string'
                values[value_structure['name']] = value_structure
            for value in question.paragraph_values.all():
                value_structure = game.serializers.ParagraphValueSerializer(value).data
                value_structure['type'] = 'paragraph'
                values[value_structure['name']] = value_structure

            question_structure['values'] = values

            questions.append(question_structure)

        questions.sort(key=lambda x: x['order'], reverse=False)

        lesson_structure['questions'] = questions

        return JsonResponse(lesson_structure)


class StudentLessonResults(APIView):
    authentication_classes = (TokenAuthentication,)
    permission_classes = (IsAuthenticated,)

    def getLesson(self, request, lesson_id):
        return LessonGrade.objects.get(course_grade__student_id=request.user.id, lesson_id=lesson_id)

    def get(self, request, lesson_id, format=None):
        lesson_grade = self.getLesson(request, lesson_id)
        if lesson_grade is None:
            return HttpResponseBadRequest("Student is not signed up for this lesson!")

        lesson_structure = {}
        lesson_structure['id'] = lesson_grade.id
        lesson_structure['lesson_id'] = lesson_grade.lesson.id
        lesson_structure['weight'] = WeightedLesson.objects.get(lesson_id=lesson_grade.lesson_id, course_id=lesson_grade.course_grade.course_id).weight
        lesson_structure['lesson_state'] = lesson_grade.get_lesson_state_display()
        lesson_structure['total_questions'] = lesson_grade.get_grades()['total_questions']
        lesson_structure['answered_questions'] = lesson_grade.get_grades()['answered_questions']
        if(lesson_grade.get_grades()['grade_max'] > 0):
            lesson_structure['grade'] = str(lesson_grade.get_grades()['grade']/lesson_grade.get_grades()['grade_max']*100)
        else:
            lesson_structure['grade'] = 'N/A'
        lesson_structure['type'] = lesson_grade.lesson.get_lesson_type_display()
        lesson_structure['name'] = lesson_grade.lesson.topic
        lesson_structure['retakes_allowed'] = lesson_grade.lesson.retakes
        lesson_structure['closable'] = lesson_grade.lesson.one_sitting

        answers = []

        for answer in lesson_grade.question_results.all():
            answer_structure = {}
            answer_structure['id'] = answer.id
            answer_structure['name'] = answer.question.name
            answer_structure['type'] = answer.question.get_question_type_display()
            answer_structure['order'] = answer.question.order
            answer_structure['mark'] = "%.2f/%d" % (answer.grade, answer.question.marks)

            answers.append(answer_structure)

        lesson_structure['answers'] = answers

        return JsonResponse(lesson_structure)


class StudentAnswer(APIView):
    authentication_classes = (TokenAuthentication,)
    permission_classes = (IsAuthenticated,)

    def get(self, request, question_id, format=None):
        (answer, created) = Answer.objects.get_or_create(question_id=question_id,
                                                         lesson_grade=LessonGrade.objects.get(lesson__included_questions__pk=question_id,
                                                                                              course_grade__student_id=request.user.id))

        answer_structure = game.serializers.AnswerSerializer(answer).data

        values = {}

        for value in answer.integer_answers.all():
            value_structure = game.serializers.IntegerAnswerSerializer(value).data
            value_structure['type'] = 'integer'
            values[value_structure['name']] = value_structure
        for value in answer.floating_point_answers.all():
            value_structure = game.serializers.FloatingPointAnswerSerializer(value).data
            value_structure['type'] = 'float'
            values[value_structure['name']] = value_structure
        for value in answer.string_answers.all():
            value_structure = game.serializers.StringAnswerSerializer(value).data
            value_structure['type'] = 'string'
            values[value_structure['name']] = value_structure
        for value in answer.paragraph_answers.all():
            value_structure = game.serializers.ParagraphAnswerSerializer(value).data
            value_structure['type'] = 'paragraph'
            values[value_structure['name']] = value_structure

        answer_structure['values'] = values

        return JsonResponse(answer_structure)

    def post(self, request, question_id, format=None):
        print(question_id)
        print(request.DATA)

        try:
            LessonGrade.objects.get(course_grade__student_id=request.user.id, lesson_id=Question.objects.get(id=question_id).lesson_id)
        except:
            return HttpResponseBadRequest('No assignment exists for the provided student and question combination')

        (answer, created) = Answer.objects.get_or_create(question_id=question_id, lesson_grade__course_grade__student_id=request.user.id)

        # Important to note! All incomming data is in string format right now. Limitation of SimpleJSON...

        if 'total_tries' in request.DATA:
            answer.total_tries = int(request.DATA['total_tries'])
            answer.calculate_grade()
        else:
            return HttpResponseBadRequest('total_tries is a required field')

        print("Dealing with values")

        for value in request.DATA['values'].values():
            if value['type'] == 'integer':
                print(answer.integer_answers.get(id=value['id']))
                serializer = game.serializers.IntegerAnswerSerializer(answer.integer_answers.get(id=value['id']), data=value)
                print(serializer)
                if(serializer.is_valid()):
                    print("valid")
                    serializer.save(answer=answer)
                else:
                    print("not valid")
            elif value['type'] == 'float':
                serializer = game.serializers.FloatingPointAnswerSerializer(answer.floating_point_answers.get(id=value['id']), data=value)
                if(serializer.is_valid()):
                    serializer.save(answer=answer)
            elif value['type'] == 'string':
                serializer = game.serializers.StringAnswerSerializer(answer.string_answers.get(id=value['id']), data=value)
                if(serializer.is_valid()):
                    serializer.save(answer=answer)
            elif value['type'] == 'paragraph':
                serializer = game.serializers.ParagraphAnswerSerializer(answer.paragraph_answers.get(id=value['id']), data=value)
                if(serializer.is_valid()):
                    serializer.save(answer=answer)
        answer.save()

        print("Done dealing with values")

        aggregates = answer.lesson_grade.get_grades()
        if aggregates['answered_questions'] == aggregates['total_questions']:
            answer.lesson_grade.lesson_state = LessonGrade.FINISHED
            answer.lesson_grade.save()

        print("All good!")

        return HttpResponse("success")


student_list_lessons = StudentListLessons.as_view()
student_lesson_details = StudentLessonDetails.as_view()
student_lesson_results = StudentLessonResults.as_view()
student_answer_details = StudentAnswer.as_view()
