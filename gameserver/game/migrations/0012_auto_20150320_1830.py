# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('game', '0011_auto_20150313_0457'),
    ]

    operations = [
        migrations.AddField(
            model_name='lesson',
            name='description',
            field=models.CharField(default=b'', max_length=4096),
            preserve_default=True,
        ),
        migrations.AddField(
            model_name='question',
            name='description',
            field=models.CharField(default=b'', max_length=4096),
            preserve_default=True,
        ),
        migrations.AlterField(
            model_name='floatingpointvalue',
            name='max_value',
            field=models.FloatField(default=1),
            preserve_default=True,
        ),
        migrations.AlterField(
            model_name='floatingpointvalue',
            name='min_value',
            field=models.FloatField(default=-1),
            preserve_default=True,
        ),
        migrations.AlterField(
            model_name='integervalue',
            name='max_value',
            field=models.IntegerField(default=1000),
            preserve_default=True,
        ),
        migrations.AlterField(
            model_name='integervalue',
            name='min_value',
            field=models.IntegerField(default=-1000),
            preserve_default=True,
        ),
    ]
