from django.contrib import admin
from models import MailingList, ContactItem


@admin.register(MailingList)
class MailingListAdmin(admin.ModelAdmin):
    def has_add_permission(self, request, obj=None):
        return False


@admin.register(ContactItem)
class ContactItemAdmin(admin.ModelAdmin):
    list_display = ('name', 'email', 'subject')

    def has_add_permission(self, request, obj=None):
        return False
