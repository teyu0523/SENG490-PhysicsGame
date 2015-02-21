from django.http import JsonResponse, HttpResponseBadRequest, HttpResponse

from rest_framework.views import APIView
from rest_framework.authentication import TokenAuthentication
from rest_framework.permissions import IsAuthenticated
# from rest_framework.renderers import JSONRenderer
# from rest_framework.parsers import JSONParser

# from game.mixins import *
from game.models import Grade, LessonGrade, WeightedLesson, Question, Answer, CannonsAnswer


class StudentListLessons(APIView):
    authentication_classes = (TokenAuthentication,)
    permission_classes = (IsAuthenticated,)

    def get(self, request):
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

    def getLesson(self, request):
        if 'id' in request.GET:
            return LessonGrade.objects.get(id=request.GET['id'])
        elif 'lesson_id' in request.GET:
            return LessonGrade.objects.get(course_grade__student_id=request.user.id, lesson_id=request.GET['lesson_id'])

    def get(self, request):
        lesson_grade = self.getLesson(request)
        if lesson_grade is None:
            return HttpResponseBadRequest()

        lesson_structure = {}
        lesson_structure['id'] = lesson_grade.id
        lesson_structure['lesson_id'] = lesson_grade.lesson.id
        lesson_structure['weight'] = WeightedLesson.objects.get(lesson_id=lesson_grade.lesson_id, course_id=lesson_grade.course_grade.course_id).weight
        lesson_structure['lesson_state'] = lesson_grade.get_lesson_state_display()
        lesson_structure['total_questions'] = lesson_grade.get_grades()['total_questions']
        lesson_structure['answered_questions'] = lesson_grade.get_grades()['answered_questions']
        if(lesson_grade.get_grades()['grade_max'] > 0):
            lesson_structure['grade'] = str(lesson_grade.get_grades()['grade']/lesson_grade.get_grades()['grade_max'])
        else:
            lesson_structure['grade'] = 'N/A'
        lesson_structure['type'] = lesson_grade.lesson.get_lesson_type_display()
        lesson_structure['name'] = lesson_grade.lesson.topic
        lesson_structure['retakes_allowed'] = lesson_grade.lesson.retakes
        lesson_structure['closable'] = lesson_grade.lesson.one_sitting

        questions = []

        for question in lesson_grade.lesson.included_questions.all():
            question_structure = {}
            question_structure['id'] = question.id
            question_structure['type'] = question.get_question_type_display()
            question_structure['order'] = question.order
            question_structure['marks'] = question.marks
            question_structure['max_tries'] = question.max_tries
            question_structure['playable'] = question.playable

            if question.question_type == Question.CANNONS:
                question_structure['player_tank_pos_x'] = question.cannons_extension.player_tank_pos_x
                question_structure['player_tank_pos_y'] = question.cannons_extension.player_tank_pos_y
                question_structure['player_tank_angle'] = question.cannons_extension.player_tank_angle
                question_structure['player_tank_velocity'] = question.cannons_extension.player_tank_velocity
                question_structure['enemy_tank_pos_x'] = question.cannons_extension.enemy_tank_pos_x
                question_structure['enemy_tank_pos_y'] = question.cannons_extension.enemy_tank_pos_y
                question_structure['enemy_tank_angle'] = question.cannons_extension.enemy_tank_angle
                question_structure['enemy_tank_velocity'] = question.cannons_extension.enemy_tank_velocity

            questions.append(question_structure)

        lesson_structure['questions'] = questions

        return JsonResponse(lesson_structure)


class StudentAnswer(APIView):
    authentication_classes = (TokenAuthentication,)
    permission_classes = (IsAuthenticated,)

    def get(self, request):
        (answer, created) = Answer.objects.get_or_create(question_id=request.GET['id'], lesson_grade__course_grade__student_id=request.user.id)
        if created:
            if answer.question.question_type == Question.CANNONS:
                CannonsAnswer.objects.create(answer=answer)

        answer_structure = {}
        answer_structure['id'] = answer.id
        answer_structure['question_id'] = answer.question.id
        answer_structure['grade'] = answer.grade
        answer_structure['total_tries'] = answer.total_tries
        if answer.question.question_type == Question.CANNONS:
            answer_structure['player_tank_pos_x'] = answer.cannons_extension.player_tank_pos_x
            answer_structure['player_tank_pos_y'] = answer.cannons_extension.player_tank_pos_y
            answer_structure['player_tank_angle'] = answer.cannons_extension.player_tank_angle
            answer_structure['player_tank_velocity'] = answer.cannons_extension.player_tank_velocity
            answer_structure['enemy_tank_pos_x'] = answer.cannons_extension.enemy_tank_pos_x
            answer_structure['enemy_tank_pos_y'] = answer.cannons_extension.enemy_tank_pos_y
            answer_structure['enemy_tank_angle'] = answer.cannons_extension.enemy_tank_angle
            answer_structure['enemy_tank_velocity'] = answer.cannons_extension.enemy_tank_velocity

        return JsonResponse(answer_structure)

    def post(self, request):
        try:
            LessonGrade.objects.get(course_grade__student_id=request.user.id, lesson_id=Question.objects.get(request.GET['id']).lesson_id)
        except:
            return HttpResponseBadRequest('total_tries is a required field')

        (answer, created) = Answer.objects.get_or_create(question_id=request.GET['id'], lesson_grade__course_grade__student_id=request.user.id)
        if created:
            if answer.question.question_type == Question.CANNONS:
                CannonsAnswer.objects.create(answer=answer)

        if 'total_tries' in request.DATA:
            answer.total_tries = request.DATA['total_tries']
            answer.calculate_grade()
        else:
            return HttpResponseBadRequest('total_tries is a required field')
        if answer.question.question_type == Question.CANNONS:
            if 'player_tank_pos_x' in request.DATA:
                answer.cannons_extension.player_tank_pos_x = request.DATA['player_tank_pos_x']
            if 'player_tank_pos_y' in request.DATA:
                answer.cannons_extension.player_tank_pos_y = request.DATA['player_tank_pos_y']
            if 'player_tank_angle' in request.DATA:
                answer.cannons_extension.player_tank_angle = request.DATA['player_tank_angle']
            if 'player_tank_velocity' in request.DATA:
                answer.cannons_extension.player_tank_velocity = request.DATA['player_tank_velocity']
            if 'enemy_tank_pos_x' in request.DATA:
                answer.cannons_extension.enemy_tank_pos_x = request.DATA['enemy_tank_pos_x']
            if 'enemy_tank_pos_y' in request.DATA:
                answer.cannons_extension.enemy_tank_pos_y = request.DATA['enemy_tank_pos_y']
            if 'enemy_tank_angle' in request.DATA:
                answer.cannons_extension.enemy_tank_angle = request.DATA['enemy_tank_angle']
            if 'enemy_tank_velocity' in request.DATA:
                answer.cannons_extension.enemy_tank_velocity = request.DATA['enemy_tank_velocity']
            answer.cannons_extension.save()
        answer.save()

        return HttpResponse()

student_list_lessons = StudentListLessons.as_view()
student_lesson_details = StudentLessonDetails.as_view()
student_answer = StudentAnswer.as_view()
