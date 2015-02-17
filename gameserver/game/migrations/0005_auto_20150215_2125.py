# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations
from django.conf import settings


class Migration(migrations.Migration):

    dependencies = [
        migrations.swappable_dependency(settings.AUTH_USER_MODEL),
        ('game', '0004_auto_20150215_2010'),
    ]

    operations = [
        migrations.CreateModel(
            name='Grade',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('grade', models.IntegerField(default=0)),
                ('grade_max', models.IntegerField(default=0)),
                ('grade_percent', models.FloatField(default=0)),
                ('taken', models.BooleanField(default=False)),
                ('course', models.ForeignKey(related_name='grades', to='game.Course')),
                ('student', models.ForeignKey(related_name='grades', to=settings.AUTH_USER_MODEL)),
            ],
            options={
            },
            bases=(models.Model,),
        ),
        migrations.RemoveField(
            model_name='grades',
            name='course',
        ),
        migrations.RemoveField(
            model_name='grades',
            name='student',
        ),
        migrations.AlterField(
            model_name='lessongrade',
            name='course_grade',
            field=models.ForeignKey(related_name='lessons', to='game.Grade'),
            preserve_default=True,
        ),
        migrations.DeleteModel(
            name='Grades',
        ),
    ]
