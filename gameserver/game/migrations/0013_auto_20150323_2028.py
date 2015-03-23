# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('game', '0012_auto_20150320_1830'),
    ]

    operations = [
        migrations.AddField(
            model_name='course',
            name='description',
            field=models.TextField(default=b'', max_length=4096),
            preserve_default=True,
        ),
        migrations.AlterField(
            model_name='lesson',
            name='description',
            field=models.TextField(default=b'', max_length=4096),
            preserve_default=True,
        ),
        migrations.AlterField(
            model_name='paragraphanswer',
            name='value',
            field=models.TextField(max_length=4096, blank=True),
            preserve_default=True,
        ),
        migrations.AlterField(
            model_name='paragraphvalue',
            name='value',
            field=models.TextField(max_length=4096, blank=True),
            preserve_default=True,
        ),
        migrations.AlterField(
            model_name='question',
            name='description',
            field=models.TextField(default=b'', max_length=4096),
            preserve_default=True,
        ),
    ]
