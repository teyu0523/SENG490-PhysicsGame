from django.http import HttpResponseRedirect
from django.views.generic import View
from gameserver import settings
from models import MailingList, ContactItem


class MailingListView(View):

    def post(self, request):
        email = request.POST.get('email', None)
        if MailingList.objects.filter(email=email).count() == 0:
            MailingList.objects.create(email=email)

        if(settings.DEBUG):
            return HttpResponseRedirect('http://localhost:8000/#cta')
        else:
            return HttpResponseRedirect('http://ggollmer.cloudapp.net/#cta')


class ContactItemView(View):

    def post(self, request):
        name = request.POST.get('name', 'anonymous')
        email = request.POST.get('email', 'unknown@unknown.com')
        subject = request.POST.get('subject', 'empty')
        message = request.POST.get('message', 'empty')
        if subject != 'empty' or message != 'empty':
            ContactItem.objects.create(name=name, email=email, subject=subject, message=message)

        if(settings.DEBUG):
            return HttpResponseRedirect('http://localhost:8000/')
        else:
            return HttpResponseRedirect('http://ggollmer.cloudapp.net/')


mailing_list = MailingListView.as_view()
contact_item = ContactItemView.as_view()
