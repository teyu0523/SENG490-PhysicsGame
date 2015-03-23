# -*- coding: utf-8 -*-
from __future__ import unicode_literals

from django.db import models, migrations


class Migration(migrations.Migration):

    dependencies = [
        ('website', '0001_initial'),
    ]

    operations = [
        migrations.CreateModel(
            name='ContactItem',
            fields=[
                ('id', models.AutoField(verbose_name='ID', serialize=False, auto_created=True, primary_key=True)),
                ('name', models.CharField(default=b'', max_length=63)),
                ('email', models.EmailField(default=b'', max_length=255)),
                ('subject', models.CharField(default=b'', max_length=255)),
                ('message', models.CharField(default=b'', max_length=4096)),
            ],
            options={
                'verbose_name': 'Contact Us Entry',
                'verbose_name_plural': 'Contact Us Entries',
            },
            bases=(models.Model,),
        ),
        migrations.AlterModelOptions(
            name='mailinglist',
            options={'verbose_name': 'Mailing List Entry', 'verbose_name_plural': 'Mailing List Entries'},
        ),
    ]
