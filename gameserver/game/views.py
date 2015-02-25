from django.http import JsonResponse, HttpResponseBadRequest, HttpResponse

from rest_framework.views import APIView
from rest_framework.authentication import TokenAuthentication
from rest_framework.permissions import IsAuthenticated

# from game.mixins import *
from game.models import Grade, LessonGrade, WeightedLesson, Question, Answer, CannonsAnswer, NumericAnswer


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

            if question.question_type == Question.NUMERIC:
                question_structure['question_text'] = question.numeric_extension.question_text
                question_structure['required_answer'] = question.numeric_extension.expected_answer
            elif question.question_type == Question.CANNONS:
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
    #authentication_classes = (TokenAuthentication,)
    #permission_classes = (IsAuthenticated,)

    def get(self, request, question_id, format=None):
        (answer, created) = Answer.objects.get_or_create(question_id=question_id,
                                                         lesson_grade=LessonGrade.objects.get(lesson__included_questions__pk=question_id,
                                                                                              course_grade__student_id=2)) #request.user.id
        if created:
            if answer.question.question_type == Question.NUMERIC:
                extension = NumericAnswer.objects.create(answer=answer)
                extension.save()
            elif answer.question.question_type == Question.CANNONS:
                extension = CannonsAnswer.objects.create(answer=answer)
                extension.save()

        answer_structure = {}
        answer_structure['id'] = answer.id
        answer_structure['question_id'] = answer.question.id
        answer_structure['grade'] = answer.grade
        answer_structure['total_tries'] = answer.total_tries
        if answer.question.question_type == Question.NUMERIC:
            answer_structure['submitted_answer'] = answer.numeric_extension.submitted_answer
        elif answer.question.question_type == Question.CANNONS:
            answer_structure['player_tank_pos_x'] = answer.cannons_extension.player_tank_pos_x
            answer_structure['player_tank_pos_y'] = answer.cannons_extension.player_tank_pos_y
            answer_structure['player_tank_angle'] = answer.cannons_extension.player_tank_angle
            answer_structure['player_tank_velocity'] = answer.cannons_extension.player_tank_velocity
            answer_structure['enemy_tank_pos_x'] = answer.cannons_extension.enemy_tank_pos_x
            answer_structure['enemy_tank_pos_y'] = answer.cannons_extension.enemy_tank_pos_y
            answer_structure['enemy_tank_angle'] = answer.cannons_extension.enemy_tank_angle
            answer_structure['enemy_tank_velocity'] = answer.cannons_extension.enemy_tank_velocity

        return JsonResponse(answer_structure)

    def post(self, request, question_id, format=None):
        print(question_id)
        print(request.DATA)

        try:
            LessonGrade.objects.get(course_grade__student_id=request.user.id, lesson_id=Question.objects.get(id=question_id).lesson_id)
        except:
            return HttpResponseBadRequest('No assignment exists for the provided student and question combination')

        (answer, created) = Answer.objects.get_or_create(question_id=question_id, lesson_grade__course_grade__student_id=request.user.id)
        if created:
            if answer.question.question_type == Question.NUMERIC:
                extension = NumericAnswer.objects.create(answer=answer)
                extension.save()
            elif answer.question.question_type == Question.CANNONS:
                extension = CannonsAnswer.objects.create(answer=answer)
                extension.save()

        # Important to note! All incomming data is in string format right now. Limitation of SimpleJSON...

        if 'total_tries' in request.DATA:
            answer.total_tries = int(request.DATA['total_tries'])
            answer.calculate_grade()
        else:
            return HttpResponseBadRequest('total_tries is a required field')

        if answer.question.question_type == Question.NUMERIC:
            if 'submitted_answer' in request.DATA:
                answer.numeric_extension.submitted_answer = int(request.DATA['submitted_answer'])
            else:
                return HttpResponseBadRequest('submitted_answer is a required field for numeric type questions')
            answer.numeric_extension.save()

        elif answer.question.question_type == Question.CANNONS:
            print("numeric type")
            if 'player_tank_pos_x' in request.DATA:
                answer.cannons_extension.player_tank_pos_x = float(request.DATA['player_tank_pos_x'])
            if 'player_tank_pos_y' in request.DATA:
                answer.cannons_extension.player_tank_pos_y = float(request.DATA['player_tank_pos_y'])
            if 'player_tank_angle' in request.DATA:
                answer.cannons_extension.player_tank_angle = float(request.DATA['player_tank_angle'])
            if 'player_tank_velocity' in request.DATA:
                answer.cannons_extension.player_tank_velocity = float(request.DATA['player_tank_velocity'])
            if 'enemy_tank_pos_x' in request.DATA:
                answer.cannons_extension.enemy_tank_pos_x = float(request.DATA['enemy_tank_pos_x'])
            if 'enemy_tank_pos_y' in request.DATA:
                answer.cannons_extension.enemy_tank_pos_y = float(request.DATA['enemy_tank_pos_y'])
            if 'enemy_tank_angle' in request.DATA:
                answer.cannons_extension.enemy_tank_angle = float(request.DATA['enemy_tank_angle'])
            if 'enemy_tank_velocity' in request.DATA:
                answer.cannons_extension.enemy_tank_velocity = float(request.DATA['enemy_tank_velocity'])
            answer.cannons_extension.save()
        answer.save()

        return HttpResponse("success")

student_list_lessons = StudentListLessons.as_view()
student_lesson_details = StudentLessonDetails.as_view()
student_answer_details = StudentAnswer.as_view()
