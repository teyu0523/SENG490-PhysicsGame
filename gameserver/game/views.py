from django.conf import settings
from django.shortcuts import render
from django.http import HttpResponse, HttpResponseServerError, Http404
from django.core.exceptions import ObjectDoesNotExist

from rest_framework import generics, status, viewsets, mixins
from rest_framework.views import APIView
from rest_framework.authtoken.models import Token
from rest_framework.authtoken.views import ObtainAuthToken
from rest_framework.authentication import TokenAuthentication
from rest_framework.permissions import IsAuthenticated, AllowAny
from rest_framework.response import Response
from rest_framework.renderers import JSONRenderer
from rest_framework.parsers import JSONParser

from game.mixins import *
from game.serializers import *
from game.models import *

def home(request):
	html = "<html><body>Hello World!</body></html>"
	return HttpResponse(html)

class TestAuthCall(generics.GenericAPIView):
	authentication_classes = (TokenAuthentication,)
	permission_classes = (IsAuthenticated,)
	#renderer_classes = (JSONRenderer, JSONRenderer)

	def get(self, request):
		#data = {"result":"success!"}
		data = "success"
		return Response(data=data);

test_auth_call = TestAuthCall.as_view();