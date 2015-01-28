# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations
from django.conf import settings


class Migration(migrations.Migration):

    dependencies = [
        migrations.swappable_dependency(settings.AUTH_USER_MODEL),
    ]

    operations = [
        migrations.CreateModel(
            name='Course',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('number', models.IntegerField()),
                ('name', models.CharField(max_length=256)),
                ('year', models.IntegerField()),
                ('instructor', models.ForeignKey(related_name='instructed_courses', to=settings.AUTH_USER_MODEL)),
                ('students', models.ManyToManyField(related_name='registered_courses', to=settings.AUTH_USER_MODEL)),
            ],
            options={
            },
            bases=(models.Model,),
        ),
        migrations.CreateModel(
            name='Lesson',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('lesson_type', models.CharField(default=b'ASG', max_length=3, choices=[(b'QUI', b'Quiz'), (b'TST', b'Test'), (b'ASG', b'Assignment'), (b'PRC', b'Practice')])),
                ('topic', models.CharField(max_length=256)),
                ('author', models.ForeignKey(related_name='authored_lessons', to=settings.AUTH_USER_MODEL)),
                ('course', models.ForeignKey(related_name='lessons', to='game.Course')),
            ],
            options={
            },
            bases=(models.Model,),
        ),
    ]
