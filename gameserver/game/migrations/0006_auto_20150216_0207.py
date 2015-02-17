# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('game', '0005_auto_20150215_2125'),
    ]

    operations = [
        migrations.CreateModel(
            name='WeightedLesson',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('weight', models.FloatField(default=10.0)),
                ('course', models.ForeignKey(to='game.Course')),
                ('lesson', models.ForeignKey(to='game.Lesson')),
            ],
            options={
            },
            bases=(models.Model,),
        ),
        migrations.RenameField(
            model_name='question',
            old_name='weight',
            new_name='marks',
        ),
        migrations.RemoveField(
            model_name='grade',
            name='grade',
        ),
        migrations.RemoveField(
            model_name='grade',
            name='grade_max',
        ),
        migrations.RemoveField(
            model_name='grade',
            name='taken',
        ),
        migrations.RemoveField(
            model_name='lesson',
            name='course',
        ),
        migrations.AddField(
            model_name='course',
            name='lessons',
            field=models.ManyToManyField(related_name='courses', through='game.WeightedLesson', to='game.Lesson', blank=True),
            preserve_default=True,
        ),
        migrations.AddField(
            model_name='lesson',
            name='total_marks',
            field=models.IntegerField(default=0),
            preserve_default=True,
        ),
        migrations.AlterField(
            model_name='answer',
            name='grade',
            field=models.FloatField(default=0),
            preserve_default=True,
        ),
        migrations.AlterField(
            model_name='lessongrade',
            name='grade',
            field=models.FloatField(default=0),
            preserve_default=True,
        ),
    ]
