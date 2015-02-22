# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations
from django.conf import settings


class Migration(migrations.Migration):

    dependencies = [
        migrations.swappable_dependency(settings.AUTH_USER_MODEL),
        ('game', '0001_initial'),
    ]

    operations = [
        migrations.CreateModel(
            name='Answer',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('total_tries', models.IntegerField()),
                ('weighted_mark', models.FloatField()),
                ('lesson_results', models.ForeignKey(related_name='answers', to='game.Lesson')),
            ],
            options={
            },
            bases=(models.Model,),
        ),
        migrations.CreateModel(
            name='CannonsAnswer',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('player_tank_pos_x', models.FloatField(null=True, blank=True)),
                ('player_tank_pos_y', models.FloatField(null=True, blank=True)),
                ('player_tank_angle', models.FloatField(null=True, blank=True)),
                ('player_tank_velocity', models.FloatField(null=True, blank=True)),
                ('enemy_tank_pos_x', models.FloatField(null=True, blank=True)),
                ('enemy_tank_pos_y', models.FloatField(null=True, blank=True)),
                ('enemy_tank_angle', models.FloatField(null=True, blank=True)),
                ('enemy_tank_velocity', models.FloatField(null=True, blank=True)),
                ('answer', models.OneToOneField(related_name='cannons_extension', to='game.Answer')),
            ],
            options={
            },
            bases=(models.Model,),
        ),
        migrations.CreateModel(
            name='CannonsQuestion',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('player_tank_pos_x', models.FloatField(null=True, blank=True)),
                ('player_tank_pos_y', models.FloatField(null=True, blank=True)),
                ('player_tank_angle', models.FloatField(null=True, blank=True)),
                ('player_tank_velocity', models.FloatField(null=True, blank=True)),
                ('enemy_tank_pos_x', models.FloatField(null=True, blank=True)),
                ('enemy_tank_pos_y', models.FloatField(null=True, blank=True)),
                ('enemy_tank_angle', models.FloatField(null=True, blank=True)),
                ('enemy_tank_velocity', models.FloatField(null=True, blank=True)),
            ],
            options={
            },
            bases=(models.Model,),
        ),
        migrations.CreateModel(
            name='LessonResults',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('lesson', models.ForeignKey(related_name='student_results', to='game.Lesson')),
                ('student', models.ForeignKey(related_name='lesson_results', to=settings.AUTH_USER_MODEL)),
            ],
            options={
            },
            bases=(models.Model,),
        ),
        migrations.CreateModel(
            name='Question',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('name', models.CharField(default=b'', max_length=128)),
                ('question_type', models.CharField(default=b'CAN', max_length=3, choices=[(b'CAN', b'Cannons')])),
                ('order', models.IntegerField()),
                ('weight', models.IntegerField()),
                ('max_tries', models.IntegerField(default=10)),
                ('playable', models.BooleanField(default=True)),
                ('lesson', models.ForeignKey(related_name='included_questions', to='game.Lesson')),
            ],
            options={
            },
            bases=(models.Model,),
        ),
        migrations.AddField(
            model_name='cannonsquestion',
            name='question',
            field=models.OneToOneField(related_name='cannons_extension', to='game.Question'),
            preserve_default=True,
        ),
        migrations.AddField(
            model_name='answer',
            name='question',
            field=models.ForeignKey(related_name='answers', to='game.Question'),
            preserve_default=True,
        ),
    ]
