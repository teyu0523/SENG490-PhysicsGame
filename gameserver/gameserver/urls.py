from django.conf.urls import patterns, include, url
from django.contrib.staticfiles.urls import staticfiles_urlpatterns
from django.contrib import admin
from rest_framework.authtoken import views

admin.site.site_header = 'Physics Game Administration'

# Examples:
# url(r'^$', 'Server.views.home', name='home'),
# url(r'^blog/', include('blog.urls')),

urlpatterns = patterns('',
                       url(r'^admin/', include(admin.site.urls)),
                       url(r'^game/auth', views.obtain_auth_token),
                       url(r'^game/lessons/$', 'game.views.student_list_lessons'),
                       )
urlpatterns += staticfiles_urlpatterns()
