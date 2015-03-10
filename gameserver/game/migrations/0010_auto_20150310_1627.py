# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('game', '0009_auto_20150225_2257'),
    ]

    operations = [
        migrations.AddField(
            model_name='numericquestion',
            name='question_hint',
            field=models.CharField(default=b'hint', max_length=256),
            preserve_default=True,
        ),
        migrations.AddField(
            model_name='numericquestion',
            name='question_text_mobile',
            field=models.CharField(max_length=256, null=True),
            preserve_default=True,
        ),
    ]
