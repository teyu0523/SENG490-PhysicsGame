# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('game', '0002_auto_20150214_0207'),
    ]

    operations = [
        migrations.AddField(
            model_name='lessonresults',
            name='grade',
            field=models.IntegerField(default=0),
            preserve_default=True,
        ),
        migrations.AddField(
            model_name='lessonresults',
            name='grade_max',
            field=models.IntegerField(default=0),
            preserve_default=True,
        ),
        migrations.AddField(
            model_name='lessonresults',
            name='percent',
            field=models.FloatField(default=0),
            preserve_default=True,
        ),
    ]
