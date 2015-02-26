# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('game', '0007_auto_20150216_2315'),
    ]

    operations = [
        migrations.CreateModel(
            name='NumericAnswer',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('submitted_answer', models.IntegerField()),
                ('answer', models.OneToOneField(related_name='numeric_extension', to='game.Answer')),
            ],
            options={
            },
            bases=(models.Model,),
        ),
        migrations.CreateModel(
            name='NumericQuestion',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('question_text', models.CharField(max_length=256)),
                ('expected_answer', models.IntegerField()),
                ('question', models.OneToOneField(related_name='numeric_extension', to='game.Question')),
            ],
            options={
            },
            bases=(models.Model,),
        ),
        migrations.AlterField(
            model_name='answer',
            name='lesson_grade',
            field=models.ForeignKey(related_name='question_results', to='game.LessonGrade'),
            preserve_default=True,
        ),
        migrations.AlterField(
            model_name='question',
            name='question_type',
            field=models.CharField(default=b'CAN', max_length=3, choices=[(b'NUM', b'Numeric'), (b'CAN', b'Cannons')]),
            preserve_default=True,
        ),
    ]
