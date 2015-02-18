from django.http import HttpResponse

from rest_framework import generics
# from rest_framework.views import APIView
from rest_framework.authentication import TokenAuthentication
from rest_framework.permissions import IsAuthenticated
from rest_framework.response import Response
# from rest_framework.renderers import JSONRenderer
# from rest_framework.parsers import JSONParser

# from game.mixins import *
from game.serializers import LessonListSerializer
from game.models import Course


def home(request):
    html = "<html><body>Hello World!</body></html>"
    return HttpResponse(html)


class TestAuthCall(generics.GenericAPIView):
    authentication_classes = (TokenAuthentication,)
    permission_classes = (IsAuthenticated,)
    # renderer_classes = (JSONRenderer, JSONRenderer)

    def get(self, request):
        # data = {"result":"success!"}
        data = "success"
        return Response(data=data)


class StudentListLessons(generics.ListAPIView):
    authentication_classes = (TokenAuthentication,)
    permission_classes = (IsAuthenticated,)
    serializer_class = LessonListSerializer

    def get_queryset(self):
        c = Course.objects.select_related('instructor')
        c = c.prefetch_related('lessons')
        return Course.objects.filter(students__pk=self.request.user.id)

test_auth_call = TestAuthCall.as_view()

student_list_lessons = StudentListLessons.as_view()
