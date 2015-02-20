from django.http import JsonResponse
from django.views.generic import View

from rest_framework import generics
from rest_framework.views import APIView
from rest_framework.authentication import TokenAuthentication
from rest_framework.permissions import IsAuthenticated
# from rest_framework.renderers import JSONRenderer
# from rest_framework.parsers import JSONParser

# from game.mixins import *
from game.serializers import LessonGradeSerializer
from game.models import Grade, LessonGrade, WeightedLesson


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


class StudentLessonDetails(generics.RetrieveAPIView):
    authentication_classes = (TokenAuthentication,)
    permission_classes = (IsAuthenticated,)
    serializer_class = LessonGradeSerializer

    def get_queryset(self):
        c = LessonGrade.objects.select_related('lesson').prefetch_related('answers').select_related('grade')
        return c.get(grade__student_id=self.request.user.id, lesson_id=self.request['lesson_id'])

student_list_lessons = StudentListLessons.as_view()
student_lesson_details = StudentLessonDetails.as_view()
