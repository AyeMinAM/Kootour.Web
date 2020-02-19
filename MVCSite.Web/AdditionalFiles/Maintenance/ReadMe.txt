--------Deployment-------
Right click Project Name "MVCSite.Web" and choose "Publish"
"!Live Kootour" is for publishing to our live site https://www.kootour.com
"KootourFront" is for publishing to our testing site https://front.kootour.com
"KootourTest" is for publishing to our second testing site https://test.kootour.com

--------Backup-------
Before deploy to "!Live Kootour", PLEASE MAKE SURE BACKUP TWO THINGS:
1. BACKUP EXISITING RUNNING FOLDER "KOOTOUR" BY MAKING A ZIP FILE OF IT. 
(our project include long path files, making a zip file is the safe way instead of copying folder)
2. BACKUP LIVE DATABASE "KOOTOUR"


--------Kootour Database-------
Note: 
Table "Promo" is used for Promoton code only. A promotion code can be applied on any tours before it gets expired.

Table "VendorPromotion" is used for recording vendor issued promotion deal on individual tours

All historical schema change is stored in txt files under this folder.

--------EmailServer Database-------
Pending emails will be stored temporarily to "QueuedEmails",
and later will be moved to "SendEmailLog" after they really get sent out

Table "Visits" is used for tracking users' url visits.