# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('game', '0008_auto_20150225_2134'),
    ]

    operations = [
        migrations.AlterField(
            model_name='numericanswer',
            name='submitted_answer',
            field=models.IntegerField(null=True),
            preserve_default=True,
        ),
    ]
