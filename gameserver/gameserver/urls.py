import settings
from django.conf.urls import patterns, include, url
from django.contrib import admin
from rest_framework.authtoken import views

admin.site.site_header = 'Inuco Administration'
admin.site.site_title = 'Administration'
admin.site.title = 'Inuco'
admin.site.index_title = 'Inuco Administration'

# Examples:
# url(r'^$', 'Server.views.home', name='home'),
# url(r'^blog/', include('blog.urls')),

urlpatterns = patterns('',
                       url(r'^admin/', include(admin.site.urls)),
                       url(r'^game/auth', views.obtain_auth_token),
                       url(r'^game/lessons/$', 'game.views.student_list_lessons'),
                       url(r'^game/lesson/(?P<course_id>\d+)/(?P<lesson_id>\d+)/$', 'game.views.student_lesson_details'),
                       url(r'^game/lesson/(?P<course_id>\d+)/(?P<lesson_id>\d+)/results/$', 'game.views.student_lesson_results'),
                       url(r'^game/lesson/answer/(?P<course_id>[\d]+)/(?P<question_id>[\d]+)/$', 'game.views.student_answer_details'),
                       url(r'^website/mailinglist/$', 'website.views.mailing_list'),
                       url(r'^website/contactus/$', 'website.views.contact_item'),
                       )
urlpatterns += url(r'^static/(?P<path>.*)$', 'django.views.static.serve', {'document_root': settings.STATIC_ROOT, 'show_indexes': True}),
urlpatterns += url(r'^$', 'django.views.static.serve', {'document_root': settings.WEBSITE_ROOT, 'path': 'index.html'}),
urlpatterns += url(r'^(?P<path>.*)$', 'django.views.static.serve', {'document_root': settings.WEBSITE_ROOT}),
