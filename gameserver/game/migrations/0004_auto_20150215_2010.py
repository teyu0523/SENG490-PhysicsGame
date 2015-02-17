# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations
from django.conf import settings


class Migration(migrations.Migration):

    dependencies = [
        migrations.swappable_dependency(settings.AUTH_USER_MODEL),
        ('game', '0003_auto_20150214_0612'),
    ]

    operations = [
        migrations.CreateModel(
            name='Grades',
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
        migrations.CreateModel(
            name='LessonGrade',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('grade', models.IntegerField(default=0)),
                ('grade_max', models.IntegerField(default=0)),
                ('grade_percent', models.FloatField(default=0)),
                ('lesson_state', models.CharField(default=b'NOT', max_length=3, choices=[(b'NOT', b'Not Started'), (b'SRT', b'Started'), (b'FIN', b'Finished')])),
                ('course_grade', models.ForeignKey(related_name='lessons', to='game.Grades')),
                ('lesson', models.ForeignKey(related_name='student_results', to='game.Lesson')),
            ],
            options={
            },
            bases=(models.Model,),
        ),
        migrations.RemoveField(
            model_name='lessonresults',
            name='lesson',
        ),
        migrations.RemoveField(
            model_name='lessonresults',
            name='student',
        ),
        migrations.DeleteModel(
            name='LessonResults',
        ),
        migrations.RemoveField(
            model_name='answer',
            name='lesson_results',
        ),
        migrations.RemoveField(
            model_name='answer',
            name='weighted_mark',
        ),
        migrations.AddField(
            model_name='answer',
            name='grade',
            field=models.IntegerField(default=0),
            preserve_default=True,
        ),
        migrations.AddField(
            model_name='answer',
            name='grade_max',
            field=models.IntegerField(default=0),
            preserve_default=True,
        ),
        migrations.AddField(
            model_name='answer',
            name='grade_percent',
            field=models.FloatField(default=0),
            preserve_default=True,
        ),
        migrations.AddField(
            model_name='answer',
            name='lesson_grade',
            field=models.ForeignKey(related_name='question_grades', default=None, to='game.LessonGrade'),
            preserve_default=False,
        ),
        migrations.AddField(
            model_name='lesson',
            name='one_sitting',
            field=models.BooleanField(default=False),
            preserve_default=True,
        ),
        migrations.AddField(
            model_name='lesson',
            name='retakes',
            field=models.BooleanField(default=True),
            preserve_default=True,
        ),
        migrations.AlterField(
            model_name='answer',
            name='total_tries',
            field=models.IntegerField(default=0),
            preserve_default=True,
        ),
        migrations.AlterField(
            model_name='course',
            name='instructor',
            field=models.ForeignKey(related_name='instructed_courses', blank=True, to=settings.AUTH_USER_MODEL),
            preserve_default=True,
        ),
        migrations.AlterField(
            model_name='course',
            name='students',
            field=models.ManyToManyField(related_name='registered_courses', to=settings.AUTH_USER_MODEL, blank=True),
            preserve_default=True,
        ),
        migrations.AlterField(
            model_name='question',
            name='order',
            field=models.IntegerField(default=0),
            preserve_default=True,
        ),
        migrations.AlterField(
            model_name='question',
            name='weight',
            field=models.IntegerField(default=1),
            preserve_default=True,
        ),
    ]
