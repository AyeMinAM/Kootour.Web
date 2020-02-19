BitBucket is our Git Version Console Server.
Please register a new Bitbucket account and askes Hansen to invite you to our team project "kootour.net"
and your username/pwd and auto generated link to synchronize with our Git server.

"Attribute Routing" combined "traditional routing rules" is used for SEO purpose, all these are specified in Global.asax

--------Cache-------
1. City dropdown list on home page is cached
2. All tours are cached for searching and autocomplete when site first gets loaded
3. Featured tours on home page is cached

--------Group Tours Page-------
Action "Tours" under "Tourist" controller supports 4 types of search modes:
1. By city name
2. By country name
3. By category name
4. By keywords

"DB-CityLandingPage.txt" under /Additional Files/Maintenance folder is used for 
specifying page title/page introduction/Blog List/tips/destinations information
Run this script will update DB for city-based group tours page
One city-based group tours page exapmle: https://www.kootour.com/Beijing
Note: If city-based group tours page lacks any infomation of page title/page introduction/Blog List/tips/destinations,
The code will resuce this situration by using corresponding information of the country where this city belongs to.


--------Email Development-------
1. "EmailGenerator" class under project "MVCSite.Biz" folder "EmailService" is used for sending our emails
2. "EmailService" class under project "MVCSite.EmailService" is used for sending our emails
