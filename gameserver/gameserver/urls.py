from django.conf.urls import patterns, include, url
from django.contrib import admin
from rest_framework.authtoken import views

admin.site.site_header = 'Physics Game Administration'

urlpatterns = patterns('',
    # Examples:
    # url(r'^$', 'Server.views.home', name='home'),
    # url(r'^blog/', include('blog.urls')),

    url(r'^admin/', include(admin.site.urls)),
    url(r'^game/auth', views.obtain_auth_token),
    url(r'^game/lessons/$', 'game.views.student_list_lessons'),

    url(r'^$', 'game.views.home', name='home'),
    url(r'^game/test/$', 'game.views.test_auth_call'),
)
