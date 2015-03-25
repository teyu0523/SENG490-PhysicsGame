from django.db import models


class MailingList(models.Model):
    email = models.EmailField(max_length=255, default="", unique=True)

    def __str__(self):
        return self.email

    class Meta:
        verbose_name = 'Mailing List Entry'
        verbose_name_plural = 'Mailing List Entries'


class ContactItem(models.Model):
    name = models.CharField(max_length=63, default="")
    email = models.EmailField(max_length=255, default="", unique=False)
    subject = models.CharField(max_length=255, default="")
    message = models.TextField(max_length=4096, default="")

    def __str__(self):
        return ("%s (%s) - %s" % (self.name, self.email, self.subject))

    class Meta:
        verbose_name = 'Contact Us Entry'
        verbose_name_plural = 'Contact Us Entries'
