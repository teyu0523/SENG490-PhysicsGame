# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('game', '0006_auto_20150216_0207'),
    ]

    operations = [
        migrations.RemoveField(
            model_name='answer',
            name='grade_max',
        ),
        migrations.RemoveField(
            model_name='answer',
            name='grade_percent',
        ),
        migrations.RemoveField(
            model_name='grade',
            name='grade_percent',
        ),
        migrations.RemoveField(
            model_name='lesson',
            name='total_marks',
        ),
        migrations.RemoveField(
            model_name='lessongrade',
            name='grade',
        ),
        migrations.RemoveField(
            model_name='lessongrade',
            name='grade_max',
        ),
        migrations.RemoveField(
            model_name='lessongrade',
            name='grade_percent',
        ),
        migrations.AlterField(
            model_name='lessongrade',
            name='course_grade',
            field=models.ForeignKey(related_name='lesson_grades', to='game.Grade'),
            preserve_default=True,
        ),
        migrations.AlterField(
            model_name='question',
            name='marks',
            field=models.IntegerField(default=0),
            preserve_default=True,
        ),
    ]
