# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('website', '0002_auto_20150323_2024'),
    ]

    operations = [
        migrations.AlterField(
            model_name='contactitem',
            name='message',
            field=models.TextField(default=b'', max_length=4096),
            preserve_default=True,
        ),
    ]
