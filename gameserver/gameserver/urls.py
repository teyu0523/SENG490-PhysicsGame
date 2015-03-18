from django.conf.urls import patterns, include, url
from django.contrib.staticfiles.urls import staticfiles_urlpatterns
from django.contrib import admin
from rest_framework.authtoken import views

admin.site.site_header = 'Inuco Administration'

# Examples:
# url(r'^$', 'Server.views.home', name='home'),
# url(r'^blog/', include('blog.urls')),

urlpatterns = patterns('',
                       url(r'^admin/', include(admin.site.urls)),
                       url(r'^game/auth', views.obtain_auth_token),
                       url(r'^game/lessons/$', 'game.views.student_list_lessons'),
                       url(r'^game/lesson/(?P<lesson_id>\d+)/$', 'game.views.student_lesson_details'),
                       url(r'^game/lesson/(?P<lesson_id>\d+)/results/$', 'game.views.student_lesson_results'),
                       url(r'^game/lesson/answer/(?P<question_id>[\d]+)/$', 'game.views.student_answer_details'),
                       )
urlpatterns += staticfiles_urlpatterns()
