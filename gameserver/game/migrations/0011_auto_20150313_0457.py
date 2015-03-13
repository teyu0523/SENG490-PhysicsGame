# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('game', '0010_auto_20150310_1627'),
    ]

    operations = [
        migrations.CreateModel(
            name='FloatingPointAnswer',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('name', models.CharField(max_length=32)),
                ('value', models.FloatField(default=0)),
                ('submitted', models.BooleanField(default=False)),
                ('answer', models.ForeignKey(related_name='floating_point_answers', to='game.Answer')),
            ],
            options={
            },
            bases=(models.Model,),
        ),
        migrations.CreateModel(
            name='FloatingPointValue',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('name', models.CharField(max_length=32)),
                ('order', models.IntegerField(default=0)),
                ('value', models.FloatField(default=0, null=True, blank=True)),
                ('min_value', models.FloatField(default=2.2250738585072014e-308)),
                ('max_value', models.FloatField(default=1.7976931348623157e+308)),
                ('menu', models.BooleanField(default=True)),
                ('editable', models.BooleanField(default=True)),
                ('question', models.ForeignKey(related_name='floating_point_values', to='game.Question')),
            ],
            options={
            },
            bases=(models.Model,),
        ),
        migrations.CreateModel(
            name='IntegerAnswer',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('name', models.CharField(max_length=32)),
                ('value', models.IntegerField(default=0)),
                ('submitted', models.BooleanField(default=False)),
                ('answer', models.ForeignKey(related_name='integer_answers', to='game.Answer')),
            ],
            options={
            },
            bases=(models.Model,),
        ),
        migrations.CreateModel(
            name='IntegerValue',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('name', models.CharField(max_length=32)),
                ('order', models.IntegerField(default=0)),
                ('value', models.IntegerField(default=0, null=True, blank=True)),
                ('min_value', models.IntegerField(default=-9223372036854775808)),
                ('max_value', models.IntegerField(default=9223372036854775807)),
                ('menu', models.BooleanField(default=True)),
                ('editable', models.BooleanField(default=True)),
                ('question', models.ForeignKey(related_name='integer_values', to='game.Question')),
            ],
            options={
            },
            bases=(models.Model,),
        ),
        migrations.CreateModel(
            name='ParagraphAnswer',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('name', models.CharField(max_length=32)),
                ('value', models.CharField(max_length=4096, blank=True)),
                ('submitted', models.BooleanField(default=False)),
                ('answer', models.ForeignKey(related_name='paragraph_answers', to='game.Answer')),
            ],
            options={
            },
            bases=(models.Model,),
        ),
        migrations.CreateModel(
            name='ParagraphValue',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('name', models.CharField(max_length=32)),
                ('order', models.IntegerField(default=0)),
                ('value', models.CharField(max_length=4096, blank=True)),
                ('max_length', models.IntegerField(default=2096)),
                ('menu', models.BooleanField(default=False)),
                ('editable', models.BooleanField(default=True)),
                ('question', models.ForeignKey(related_name='paragraph_values', to='game.Question')),
            ],
            options={
            },
            bases=(models.Model,),
        ),
        migrations.CreateModel(
            name='StringAnswer',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('name', models.CharField(max_length=32)),
                ('value', models.CharField(max_length=256, blank=True)),
                ('submitted', models.BooleanField(default=False)),
                ('answer', models.ForeignKey(related_name='string_answers', to='game.Answer')),
            ],
            options={
            },
            bases=(models.Model,),
        ),
        migrations.CreateModel(
            name='StringValue',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('name', models.CharField(max_length=32)),
                ('order', models.IntegerField(default=0)),
                ('value', models.CharField(max_length=256, blank=True)),
                ('max_length', models.IntegerField(default=256)),
                ('menu', models.BooleanField(default=False)),
                ('editable', models.BooleanField(default=True)),
                ('question', models.ForeignKey(related_name='string_values', to='game.Question')),
            ],
            options={
            },
            bases=(models.Model,),
        ),
        migrations.RemoveField(
            model_name='cannonsanswer',
            name='answer',
        ),
        migrations.DeleteModel(
            name='CannonsAnswer',
        ),
        migrations.RemoveField(
            model_name='cannonsquestion',
            name='question',
        ),
        migrations.DeleteModel(
            name='CannonsQuestion',
        ),
        migrations.RemoveField(
            model_name='numericanswer',
            name='answer',
        ),
        migrations.DeleteModel(
            name='NumericAnswer',
        ),
        migrations.RemoveField(
            model_name='numericquestion',
            name='question',
        ),
        migrations.DeleteModel(
            name='NumericQuestion',
        ),
        migrations.AlterUniqueTogether(
            name='stringvalue',
            unique_together=set([('question', 'name')]),
        ),
        migrations.AlterUniqueTogether(
            name='stringanswer',
            unique_together=set([('answer', 'name')]),
        ),
        migrations.AlterUniqueTogether(
            name='paragraphvalue',
            unique_together=set([('question', 'name')]),
        ),
        migrations.AlterUniqueTogether(
            name='paragraphanswer',
            unique_together=set([('answer', 'name')]),
        ),
        migrations.AlterUniqueTogether(
            name='integervalue',
            unique_together=set([('question', 'name')]),
        ),
        migrations.AlterUniqueTogether(
            name='integeranswer',
            unique_together=set([('answer', 'name')]),
        ),
        migrations.AlterUniqueTogether(
            name='floatingpointvalue',
            unique_together=set([('question', 'name')]),
        ),
        migrations.AlterUniqueTogether(
            name='floatingpointanswer',
            unique_together=set([('answer', 'name')]),
        ),
    ]
