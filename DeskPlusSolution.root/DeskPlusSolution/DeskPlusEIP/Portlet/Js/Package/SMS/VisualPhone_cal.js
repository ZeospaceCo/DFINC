var yearSelect;
var monthSelect;
var todayDate;
var calFocus = '';
var calFocus2 = '';
var color_1 = "#CF0000";
var color_2 = "#0867A7";
var color_3 = "#86B20E";
var color_4 = "#66666C";

if (typeof(headerfooter_time_year) != "undefined") { 
	/* 오늘의 날짜를 서버 날짜로 설정 */
	todayDate = new Date(
					headerfooter_time_year, headerfooter_time_month - 1,
					headerfooter_time_day, headerfooter_time_hour,
					headerfooter_time_minute, headerfooter_time_second);
}
else
	todayDate = new Date();

function memorialDay(name, month, day, solarLunar, holiday, type) { 
	this.name = name;
	this.month = month;
	this.day = day;
	this.solarLunar = solarLunar;
	this.holiday = holiday;	/* true : 빨간날 false : 안빨간날 */
	this.type = type;	/* true : real time setting */
	this.techneer = true;
}

var memorialDays = Array (
	new memorialDay(NewYear, 1, 1, 1, true),
	new memorialDay("", 12, 0, 2, true, true),	/* 실시간으로 정해짐 */
	new memorialDay(NewYearsDay, 1, 1, 2, true),
	new memorialDay("", 1, 2, 2, true),
	new memorialDay(SamilIndependence, 3, 1, 1, true),
	new memorialDay(Buddhism, 4, 8, 2, true),
	new memorialDay(ChildDay, 5, 5, 1, true),
	new memorialDay(MemorialDay, 6, 6, 1, true),
	new memorialDay(Constituent, 7, 17, 1, true),
	new memorialDay(KoreaIndependenceDay, 8, 15, 1, true),
	new memorialDay("", 8, 14, 2, true),
	new memorialDay(ThanksgivingDay, 8, 15, 2, true),
	new memorialDay("", 8, 16, 2, true),
	new memorialDay(NationalFoundationDay, 10, 3, 1, true),
	new memorialDay(Christmas, 12, 25, 1, true),

	new memorialDay(FirstMonth15, 1, 15, 2, false),
	new memorialDay(ArborDay, 4, 5, 1, false),
	new memorialDay(Danofestival, 5, 5, 2, false),
	new memorialDay(ArmyDay, 10, 1, 1, false),
	new memorialDay(KoreanAlphabet, 10, 9, 1, false),
	new memorialDay(Accident625, 6, 25, 1, false),
	new memorialDay(ThirdLunarMonth, 3, 3, 2, false),
	new memorialDay(WaterDay, 3, 22, 1, false),
	new memorialDay(FoolsDay, 4, 1, 1, false),
	new memorialDay(HandicappedDay, 4, 20, 1, false),
	new memorialDay(ScienceDay, 4, 21, 1 , false),
	new memorialDay(LeeSunSinDay, 4, 28, 1, false),
	new memorialDay(LawDay, 5, 1, 1, false),
	new memorialDay(ParentsDay, 5, 8, 1, false),
	new memorialDay(TeacherDay, 5, 15, 1, false),
	new memorialDay(InventionDay, 5, 19, 1, false),
	new memorialDay(SeaDay, 5, 31, 1, false),
	new memorialDay(EnvironmentDay, 6, 5, 1, false),
	new memorialDay(NippleDay, 6, 15, 2, false),
	new memorialDay(SevenMonthDay, 7, 7, 2, false),
	new memorialDay(CenterDay, 9, 9, 2, false),
	new memorialDay(RailDay, 9, 18, 1, false),
	new memorialDay(FireDay, 11, 9, 1, false)
);

function myDate(year, month, day, leapMonth) { 
	this.year = year;
	this.month = month;
	this.day = day;
	this.leapMonth = leapMonth;
}

// 음력 데이터 (평달 - 작은달 :1,  큰달:2 )
// (윤달이 있는 달 - 평달이 작고 윤달도 작으면 :3 , 평달이 작고 윤달이 크면 : 4)
// (윤달이 있는 달 - 평달이 크고 윤달이 작으면 :5,  평달과 윤달이 모두 크면 : 6)
var lunarMonthTable = [
[2, 1, 2, 1, 2, 1, 2, 2, 1, 2, 1, 2],
[1, 2, 1, 1, 2, 1, 2, 5, 2, 2, 1, 2],
[1, 2, 1, 1, 2, 1, 2, 1, 2, 2, 2, 1],   /* 1901 */
[2, 1, 2, 1, 1, 2, 1, 2, 1, 2, 2, 2],
[1, 2, 1, 2, 3, 2, 1, 1, 2, 2, 1, 2],
[2, 2, 1, 2, 1, 1, 2, 1, 1, 2, 2, 1],
[2, 2, 1, 2, 2, 1, 1, 2, 1, 2, 1, 2],
[1, 2, 2, 4, 1, 2, 1, 2, 1, 2, 1, 2],
[1, 2, 1, 2, 1, 2, 2, 1, 2, 1, 2, 1],
[2, 1, 1, 2, 2, 1, 2, 1, 2, 2, 1, 2],
[1, 5, 1, 2, 1, 2, 1, 2, 2, 2, 1, 2],
[1, 2, 1, 1, 2, 1, 2, 1, 2, 2, 2, 1],
[2, 1, 2, 1, 1, 5, 1, 2, 2, 1, 2, 2],   /* 1911 */
[2, 1, 2, 1, 1, 2, 1, 1, 2, 2, 1, 2],
[2, 2, 1, 2, 1, 1, 2, 1, 1, 2, 1, 2],
[2, 2, 1, 2, 5, 1, 2, 1, 2, 1, 1, 2],
[2, 1, 2, 2, 1, 2, 1, 2, 1, 2, 1, 2],
[1, 2, 1, 2, 1, 2, 2, 1, 2, 1, 2, 1],
[2, 3, 2, 1, 2, 2, 1, 2, 2, 1, 2, 1],
[2, 1, 1, 2, 1, 2, 1, 2, 2, 2, 1, 2],
[1, 2, 1, 1, 2, 1, 5, 2, 2, 1, 2, 2],
[1, 2, 1, 1, 2, 1, 1, 2, 2, 1, 2, 2],
[2, 1, 2, 1, 1, 2, 1, 1, 2, 1, 2, 2],   /* 1921 */
[2, 1, 2, 2, 3, 2, 1, 1, 2, 1, 2, 2],
[1, 2, 2, 1, 2, 1, 2, 1, 2, 1, 1, 2],
[2, 1, 2, 1, 2, 2, 1, 2, 1, 2, 1, 1],
[2, 1, 2, 5, 2, 1, 2, 2, 1, 2, 1, 2],
[1, 1, 2, 1, 2, 1, 2, 2, 1, 2, 2, 1],
[2, 1, 1, 2, 1, 2, 1, 2, 2, 1, 2, 2],
[1, 5, 1, 2, 1, 1, 2, 2, 1, 2, 2, 2],
[1, 2, 1, 1, 2, 1, 1, 2, 1, 2, 2, 2],
[1, 2, 2, 1, 1, 5, 1, 2, 1, 2, 2, 1],
[2, 2, 2, 1, 1, 2, 1, 1, 2, 1, 2, 1],   /* 1931 */
[2, 2, 2, 1, 2, 1, 2, 1, 1, 2, 1, 2],
[1, 2, 2, 1, 6, 1, 2, 1, 2, 1, 1, 2],
[1, 2, 1, 2, 2, 1, 2, 2, 1, 2, 1, 2],
[1, 1, 2, 1, 2, 1, 2, 2, 1, 2, 2, 1],
[2, 1, 4, 1, 2, 1, 2, 1, 2, 2, 2, 1],
[2, 1, 1, 2, 1, 1, 2, 1, 2, 2, 2, 1],
[2, 2, 1, 1, 2, 1, 4, 1, 2, 2, 1, 2],
[2, 2, 1, 1, 2, 1, 1, 2, 1, 2, 1, 2],
[2, 2, 1, 2, 1, 2, 1, 1, 2, 1, 2, 1],
[2, 2, 1, 2, 2, 4, 1, 1, 2, 1, 2, 1],   /* 1941 */
[2, 1, 2, 2, 1, 2, 2, 1, 2, 1, 1, 2],
[1, 2, 1, 2, 1, 2, 2, 1, 2, 2, 1, 2],
[1, 1, 2, 4, 1, 2, 1, 2, 2, 1, 2, 2],
[1, 1, 2, 1, 1, 2, 1, 2, 2, 2, 1, 2],
[2, 1, 1, 2, 1, 1, 2, 1, 2, 2, 1, 2],
[2, 5, 1, 2, 1, 1, 2, 1, 2, 1, 2, 2],
[2, 1, 2, 1, 2, 1, 1, 2, 1, 2, 1, 2],
[2, 2, 1, 2, 1, 2, 3, 2, 1, 2, 1, 2],
[2, 1, 2, 2, 1, 2, 1, 1, 2, 1, 2, 1],
[2, 1, 2, 2, 1, 2, 1, 2, 1, 2, 1, 2],   /* 1951 */
[1, 2, 1, 2, 4, 2, 1, 2, 1, 2, 1, 2],
[1, 2, 1, 1, 2, 2, 1, 2, 2, 1, 2, 2],
[1, 1, 2, 1, 1, 2, 1, 2, 2, 1, 2, 2],
[2, 1, 4, 1, 1, 2, 1, 2, 1, 2, 2, 2],
[1, 2, 1, 2, 1, 1, 2, 1, 2, 1, 2, 2],
[2, 1, 2, 1, 2, 1, 1, 5, 2, 1, 2, 2],
[1, 2, 2, 1, 2, 1, 1, 2, 1, 2, 1, 2],
[1, 2, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1],
[2, 1, 2, 1, 2, 5, 2, 1, 2, 1, 2, 1],
[2, 1, 2, 1, 2, 1, 2, 2, 1, 2, 1, 2],   /* 1961 */
[1, 2, 1, 1, 2, 1, 2, 2, 1, 2, 2, 1],
[2, 1, 2, 3, 2, 1, 2, 1, 2, 2, 2, 1],
[2, 1, 2, 1, 1, 2, 1, 2, 1, 2, 2, 2],
[1, 2, 1, 2, 1, 1, 2, 1, 1, 2, 2, 2],
[1, 2, 5, 2, 1, 1, 2, 1, 1, 2, 2, 1],
[2, 2, 1, 2, 2, 1, 1, 2, 1, 2, 1, 2],
[1, 2, 2, 1, 2, 1, 5, 2, 1, 2, 1, 2],
[1, 2, 1, 2, 1, 2, 2, 1, 2, 1, 2, 1],
[2, 1, 1, 2, 2, 1, 2, 1, 2, 2, 1, 2],
[1, 2, 1, 1, 5, 2, 1, 2, 2, 2, 1, 2],   /* 1971 */
[1, 2, 1, 1, 2, 1, 2, 1, 2, 2, 2, 1],
[2, 1, 2, 1, 1, 2, 1, 1, 2, 2, 2, 1],
[2, 2, 1, 5, 1, 2, 1, 1, 2, 2, 1, 2],
[2, 2, 1, 2, 1, 1, 2, 1, 1, 2, 1, 2],
[2, 2, 1, 2, 1, 2, 1, 5, 2, 1, 1, 2],
[2, 1, 2, 2, 1, 2, 1, 2, 1, 2, 1, 1],
[2, 2, 1, 2, 1, 2, 2, 1, 2, 1, 2, 1],
[2, 1, 1, 2, 1, 6, 1, 2, 2, 1, 2, 1],
[2, 1, 1, 2, 1, 2, 1, 2, 2, 1, 2, 2],
[1, 2, 1, 1, 2, 1, 1, 2, 2, 1, 2, 2],   /* 1981 */
[2, 1, 2, 3, 2, 1, 1, 2, 2, 1, 2, 2],
[2, 1, 2, 1, 1, 2, 1, 1, 2, 1, 2, 2],
[2, 1, 2, 2, 1, 1, 2, 1, 1, 5, 2, 2],
[1, 2, 2, 1, 2, 1, 2, 1, 1, 2, 1, 2],
[1, 2, 2, 1, 2, 2, 1, 2, 1, 2, 1, 1],
[2, 1, 2, 2, 1, 5, 2, 2, 1, 2, 1, 2],
[1, 1, 2, 1, 2, 1, 2, 2, 1, 2, 2, 1],
[2, 1, 1, 2, 1, 2, 1, 2, 2, 1, 2, 2],
[1, 2, 1, 1, 5, 1, 2, 1, 2, 2, 2, 2],
[1, 2, 1, 1, 2, 1, 1, 2, 1, 2, 2, 2],   /* 1991 */
[1, 2, 2, 1, 1, 2, 1, 1, 2, 1, 2, 2],
[1, 2, 5, 2, 1, 2, 1, 1, 2, 1, 2, 1],
[2, 2, 2, 1, 2, 1, 2, 1, 1, 2, 1, 2],
[1, 2, 2, 1, 2, 2, 1, 5, 2, 1, 1, 2],
[1, 2, 1, 2, 2, 1, 2, 1, 2, 2, 1, 2],
[1, 1, 2, 1, 2, 1, 2, 2, 1, 2, 2, 1],
[2, 1, 1, 2, 3, 2, 2, 1, 2, 2, 2, 1],
[2, 1, 1, 2, 1, 1, 2, 1, 2, 2, 2, 1],
[2, 2, 1, 1, 2, 1, 1, 2, 1, 2, 2, 1],
[2, 2, 2, 3, 2, 1, 1, 2, 1, 2, 1, 2],   /* 2001 */
[2, 2, 1, 2, 1, 2, 1, 1, 2, 1, 2, 1],
[2, 2, 1, 2, 2, 1, 2, 1, 1, 2, 1, 2],
[1, 5, 2, 2, 1, 2, 1, 2, 1, 2, 1, 2],
[1, 2, 1, 2, 1, 2, 2, 1, 2, 2, 1, 1],
[2, 1, 2, 1, 2, 1, 5, 2, 2, 1, 2, 2],
[1, 1, 2, 1, 1, 2, 1, 2, 2, 2, 1, 2],
[2, 1, 1, 2, 1, 1, 2, 1, 2, 2, 1, 2],
[2, 2, 1, 1, 5, 1, 2, 1, 2, 1, 2, 2],
[2, 1, 2, 1, 2, 1, 1, 2, 1, 2, 1, 2],
[2, 1, 2, 2, 1, 2, 1, 1, 2, 1, 2, 1],   /* 2011 */
[2, 1, 6, 2, 1, 2, 1, 1, 2, 1, 2, 1],
[2, 1, 2, 2, 1, 2, 1, 2, 1, 2, 1, 2],
[1, 2, 1, 2, 1, 2, 1, 2, 5, 2, 1, 2],
[1, 2, 1, 1, 2, 1, 2, 2, 2, 1, 2, 1],
[2, 1, 2, 1, 1, 2, 1, 2, 2, 1, 2, 2],
[2, 1, 1, 2, 3, 2, 1, 2, 1, 2, 2, 2],
[1, 2, 1, 2, 1, 1, 2, 1, 2, 1, 2, 2],
[2, 1, 2, 1, 2, 1, 1, 2, 1, 2, 1, 2],
[2, 1, 2, 5, 2, 1, 1, 2, 1, 2, 1, 2],
[1, 2, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1],   /* 2021 */
[2, 1, 2, 1, 2, 2, 1, 2, 1, 2, 1, 2],
[1, 5, 2, 1, 2, 1, 2, 2, 1, 2, 1, 2],
[1, 2, 1, 1, 2, 1, 2, 2, 1, 2, 2, 1],
[2, 1, 2, 1, 1, 5, 2, 1, 2, 2, 2, 1],
[2, 1, 2, 1, 1, 2, 1, 2, 1, 2, 2, 2],
[1, 2, 1, 2, 1, 1, 2, 1, 1, 2, 2, 2],
[1, 2, 2, 1, 5, 1, 2, 1, 1, 2, 2, 1],
[2, 2, 1, 2, 2, 1, 1, 2, 1, 1, 2, 2],
[1, 2, 1, 2, 2, 1, 2, 1, 2, 1, 2, 1],
[2, 1, 5, 2, 1, 2, 2, 1, 2, 1, 2, 1],   /* 2031 */
[2, 1, 1, 2, 1, 2, 2, 1, 2, 2, 1, 2],
[1, 2, 1, 1, 2, 1, 2, 1, 2, 2, 5, 2],
[1, 2, 1, 1, 2, 1, 2, 1, 2, 2, 2, 1],
[2, 1, 2, 1, 1, 2, 1, 1, 2, 2, 1, 2],
[2, 2, 1, 2, 1, 4, 1, 1, 2, 2, 1, 2],
[2, 2, 1, 2, 1, 1, 2, 1, 1, 2, 1, 2],
[2, 2, 1, 2, 1, 2, 1, 2, 1, 1, 2, 1],
[2, 2, 1, 2, 5, 2, 1, 2, 1, 2, 1, 1],
[2, 1, 2, 2, 1, 2, 2, 1, 2, 1, 2, 1],
[2, 1, 1, 2, 1, 2, 2, 1, 2, 2, 1, 2],   /* 2041 */
[1, 5, 1, 2, 1, 2, 1, 2, 2, 2, 1, 2],
[1, 2, 1, 1, 2, 1, 1, 2, 2, 1, 2, 2]];
/* 양력 <-> 음력 변환 함수
 * type : 1 - 양력 -> 음력
 *        2 - 음력 -> 양력
 * leapmonth : 0 - 평달
 *             1 - 윤달 (type = 2 일때만 유효)
*/ 

function lunarCalc(year, month, day, type, leapmonth) { 
	var solYear, solMonth, solDay;
	var lunYear, lunMonth, lunDay;
	var lunLeapMonth, lunMonthDay;	
	var i, lunIndex;

	var solMonthDay = [31, 0, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

	/* range check */
	if (year < 1900 || year > 2040)
	{
		alert(Use2040_MSG);
		return;
	}

	/* 속도 개선을 위해 기준 일자를 2000년으로 */
	if (year >= 2000)
	{
		/* 기준일자 양력 2000년 1월 1일 (음력 1999년 11월 25일) */
		solYear = 2000;
		solMonth = 1;
		solDay = 1;
		lunYear = 1999;
		lunMonth = 11;
		lunDay = 25;
		lunLeapMonth = 0;

		solMonthDay[1] = 29;	/* 2000 년 2월 28일 */
		lunMonthDay = 30;	/* 1999년 11월 */
	}

	lunIndex = lunYear - 1899;

	while (true)
	{
//		document.write(solYear + "-" + solMonth + "-" + solDay + "<->");
//		document.write(lunYear + "-" + lunMonth + "-" + lunDay + " " + lunLeapMonth + " " + lunMonthDay + "<br>");

		if (type == 1 &&
			year == solYear &&
			month == solMonth &&
			day == solDay)
		{
			return new myDate(lunYear, lunMonth, lunDay, lunLeapMonth);
		}	
		else if (type == 2 &&
				year == lunYear &&
				month == lunMonth &&
				day == lunDay && 
				leapmonth == lunLeapMonth)
		{
			return new myDate(solYear, solMonth, solDay, 0);
		}

		/* add a day of solar calendar */
		if (solMonth == 12 && solDay == 31)
		{
			solYear++;
			solMonth = 1;
			solDay = 1;

			/* set monthDay of Feb */
			if (solYear % 400 == 0)
				solMonthDay[1] = 29;
			else if (solYear % 100 == 0)
				solMonthDay[1] = 28;
			else if (solYear % 4 == 0)
				solMonthDay[1] = 29;
			else
				solMonthDay[1] = 28;

		}
		else if (solMonthDay[solMonth - 1] == solDay)
		{
			solMonth++;
			solDay = 1;	
		}
		else
			solDay++;

		/* add a day of lunar calendar */
		if (lunMonth == 12 &&
			((lunarMonthTable[lunIndex][lunMonth - 1] == 1 && lunDay == 29) ||
			(lunarMonthTable[lunIndex][lunMonth - 1] == 2 && lunDay == 30)))
		{
			lunYear++;
			lunMonth = 1;
			lunDay = 1;

			if (lunYear > 2043) {
				alert(NoInputMonth_MSG);
				break;
			}

			lunIndex = lunYear - 1899;

			if (lunarMonthTable[lunIndex][lunMonth - 1] == 1)
				lunMonthDay = 29;
			else if (lunarMonthTable[lunIndex][lunMonth - 1] == 2)
				lunMonthDay = 30;
		}
		else if (lunDay == lunMonthDay)
		{
			if (lunarMonthTable[lunIndex][lunMonth - 1] >= 3
				&& lunLeapMonth == 0)
			{
				lunDay = 1;
				lunLeapMonth = 1;
			}
			else
			{
				lunMonth++;
				lunDay = 1;
				lunLeapMonth = 0;
			}

			if (lunarMonthTable[lunIndex][lunMonth - 1] == 1)
				lunMonthDay = 29;
			else if (lunarMonthTable[lunIndex][lunMonth - 1] == 2)
				lunMonthDay = 30;
			else if (lunarMonthTable[lunIndex][lunMonth - 1] == 3)
				lunMonthDay = 29;
			else if (lunarMonthTable[lunIndex][lunMonth - 1] == 4 &&
					lunLeapMonth == 0)
				lunMonthDay = 29;
			else if (lunarMonthTable[lunIndex][lunMonth - 1] == 4 &&
					lunLeapMonth == 1)
				lunMonthDay = 30;
			else if (lunarMonthTable[lunIndex][lunMonth - 1] == 5 &&
					lunLeapMonth == 0)
				lunMonthDay = 30;
			else if (lunarMonthTable[lunIndex][lunMonth - 1] == 5 &&
					lunLeapMonth == 1)
					lunMonthDay = 29;
			else if (lunarMonthTable[lunIndex][lunMonth - 1] == 6)
				lunMonthDay = 30;
		}
		else
			lunDay++;
	}
}

function getWeekday(year, month, day) { 

	var weekday = Array(Sunday_MSG, Monday_MSG, Tuesday_MSG, Wendensday_MSG, Thursday_MSG, Friday_MSG, Saturday_MSG);
	var date = new Date(year, month - 1, day);

	if (date)
		return weekday[date.getDay()];
}

function getPassDay(year, month, day) { 
	var date = new Date(year, month - 1, day);

	var interval = Math.floor((todayDate - date) / (1000 * 60 * 60 * 24) + 1);

	return interval;
}

function getDDay(year, month, day) { 
	var date = new Date(year, month - 1, day);

	var interval = Math.floor((date - todayDate) / (1000 * 60 * 60 * 24) + 1);

	return interval;
}

function getDateSpecificInterval(year, month, day, interval) { 
	var date = new Date(year, month - 1, parseInt(day, 10) + parseInt(interval, 10) - 1);

	return date;
}

function dayCalcDisplay(type) { 
	switch (type) {
		/* 오늘은 몇일째 */
		case 1:
			var startYear = parseInt(document.getElementById("mStartYear").value, 10);
			var startMonth = parseInt(document.getElementById("mStartMonth").value, 10);
			var startDay = parseInt(document.getElementById("mStartDay").value, 10);
			break;

		/* x 일째 되는날 */
		case 2:
			var startYear = parseInt(document.getElementById("mmStartYear").value, 10);
			var startMonth = parseInt(document.getElementById("mmStartMonth").value, 10);
			var startDay = parseInt(document.getElementById("mmStartDay").value, 10);
			break;

		/* D-day */
		case 3:	
			var startYear = parseInt(document.getElementById("targetYear").value, 10);
			var startMonth = parseInt(document.getElementById("targetMonth").value, 10);
			var startDay = parseInt(document.getElementById("targetDay").value, 10);
			break;
			
		/* 요일 계산 */			
		case 4:	
			var startYear = parseInt(document.getElementById("weekdayYear").value, 10);
			var startMonth = parseInt(document.getElementById("weekdayMonth").value, 10);
			var startDay = parseInt(document.getElementById("weekdayDay").value, 10);
			break;

		/* 양력 음력 변환 */			
		case 5:
			var startYear = parseInt(document.getElementById("lsStartYear").value, 10);
			var startMonth = parseInt(document.getElementById("lsStartMonth").value, 10);
			var startDay = parseInt(document.getElementById("lsStartDay").value, 10);
			break;
	}
	
	
	if ( !startYear || startYear == 0 ||
		 !startMonth || startMonth == 0 ||
		 !startDay || startDay == 0 )
	{
		alert(NeedDate_MSG);
		return;
	}

	var solMonthDay = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

	if (startYear % 400 == 0 || ( startYear % 4 == 0 && startYear % 100 != 0 )) solMonthDay[1] += 1;


	if ( startMonth < 1 || startMonth > 12 ||
		 startDay < 1 || startDay > solMonthDay[startMonth-1] ) {
		if ( solMonthDay[1] == 28 && startMonth == 2 && startDay > 28 ) 
			alert(LeapYear_MSG);
		else
			alert(DateLimit_MSG);
		return;
	}

	var startDate = new Date(startYear, startMonth - 1, startDay);
	var today = new Date(todayDate.getFullYear(),
						todayDate.getMonth(), todayDate.getDate());

	switch (type)
	{
	/* 오늘은 몇일째 */
	case 1:
		if (today < startDate)
		{
			alert(BeforeDate_MSG);
			return;
		}

		var interval = getPassDay(startYear, startMonth, startDay);

		document.getElementById("day1").value = interval;
		break;
	/* x 일 되는 날은 */
	case 2:
		/*	
		if (today < startDate)
		{
			alert("기준일을 오늘 이전 날짜로 설정하세요");
			return;
		}
		*/

		var day2 = document.getElementById("day2").value;

		if (day2 <= 0)
		{
			alert(MoreThanZero_MSG);
			return;
		}

		var date = getDateSpecificInterval(startYear, startMonth, startDay, day2);

		document.getElementById("resultDay").value = 
			String(date.getFullYear()) + Year_MSG + " " + 
			String(date.getMonth() +1 ) + Monday_MSG + " " + 
			String(date.getDate()) + Sunday_MSG;

		break;	
	/* D-Day */
	case 3:
		
		var interval = getDDay(startYear, startMonth, startDay);

		if (today > startDate)
		{
			alert(SetNextDay_MSG);
			return;
		}

		document.getElementById("day3").value = interval;

		break;

	/* 요일 계산 */
	case 4:
		if (startYear < 100)
		{
			alert(NextYear100_MSG);
			return;
		}
		weekday.value = getWeekday(startYear, startMonth, startDay) + DayWeek_MSG;

		break;

	/* 양력/음력 변환 */
	case 5:
		if (document.getElementById('solarLunar').value == 'solar')
		{
			var leapMonth = document.getElementById('leapMonth').checked;
			var date = lunarCalc(startYear, startMonth, startDay, 2, leapMonth);
		}
		else
		{
			var date = lunarCalc(startYear, startMonth, startDay, 1);
		}

		if (date)
		{
			document.getElementById('solarLunarDay').value = 
				date.year + Year_MSG + " " + 
				(date.leapMonth ? Leap_MSG : "") + date.month + Monday_MSG + " " +
				date.day + Sunday_MSG + " ";

		}
		else
		{
			document.getElementById('solarLunarDay').value = "";
		}

		break;
	}
}

function memorialDayCheck(solarDate, lunarDate) { 
	var i;
	var memorial;


	for (i = 0;i < memorialDays.length;i++)
	{
		if (memorialDays[i].month == solarDate.month &&
			memorialDays[i].day == solarDate.day &&
			memorialDays[i].solarLunar == 1)
			return memorialDays[i];

		if (memorialDays[i].month == lunarDate.month &&
			memorialDays[i].day == lunarDate.day &&
			memorialDays[i].solarLunar == 2 &&
			!memorialDays[i].leapMonth)
			return memorialDays[i];
	}

	return null;
}

/*


function setStartDate(year, month, day) { 
	document.getElementById('startYear').value = year;
	document.getElementById('startMonth').value = month;
	document.getElementById('startDay').value = day;
}
*/

function setCalendar(year, month) { 
	var i;
	var oYearSelect = document.getElementById('yearSelect');
	var oMonthSelect = document.getElementById('monthSelect');

	if (!year)
	{
		year = oYearSelect.value;
		month = oMonthSelect.value;
	}
	else
	{
		for (i = 0;i < oYearSelect.length;i++)
			if (oYearSelect[i].value == year)
			{
				oYearSelect.selectedIndex = i;
				break;
			}
	
		for (i = 0;i < oMonthSelect.length;i++)
			if (oMonthSelect[i].value == month)
			{
				oMonthSelect.selectedIndex = i;
				break;
			}
	}

	var monthDay = Array(31, 0, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31);
	var wwck = "";

	/* set monthDay of Feb */
	if (year % 400 == 0)
		monthDay[1] = 29;
	else if (year % 100 == 0)
		monthDay[1] = 28;
	else if (year % 4 == 0)
		monthDay[1] = 29;
	else
		monthDay[1] = 28;


	/* set the day before 설날 */
	if (lunarMonthTable[year - 1 - 1899][11] == 1)
		memorialDays[1].day = 29;
	else if (lunarMonthTable[year - 1 - 1899][11] == 2)
		memorialDays[1].day = 30;

	var date = new Date(year, month - 1, 1);
	var startWeekday = date.getDay();

	/* clean all day cell */
	for (i = 0;i < 42;i++)
	{
		document.getElementById('dayCell' + i).innerHTML = "";
//		document.getElementById('memoCell' + i).innerHTML = "";
	}

	/* fill day cell */		
	for (i = 0;i < monthDay[month - 1];i ++)
	{
		var index = startWeekday + i;
		var dayHTML;
		var memoHTML;

		var solarDate = new myDate(year, month, i + 1);
		var lunarDate = lunarCalc(year, month, i + 1, 1);

		/* memorial day */
		var memorial = memorialDayCheck(solarDate, lunarDate);

		/* 쉬지않는 기념일 */
		var memorialDay = false;
		if (memorial && memorial.holiday == false)
			memorialDay = true;

		if (memorial && memorial.holiday == true) {
			wwck = 0;
		} else {
			wwck = index % 7;
		}

		/* day print */
		dayHTML = "<a href='javascript:calinput("+(i+1)+","+index+","+(wwck)+");'><font color='COLOR' title='TITLE'>HIGHLIGHT_START" +
		    ( i + 1 ) + "HIGHLIGHT_END</font></a>";

		/* decoration */
		if ((memorial && memorial.holiday) || wwck == 0)
			dayHTML = dayHTML.replace("COLOR", color_1);
		else if (wwck == 6)
			dayHTML = dayHTML.replace("COLOR", color_2);
		else
			dayHTML = dayHTML.replace("COLOR", color_4);

		if (memorial)
			dayHTML = dayHTML.replace("TITLE", memorial.name);

		if (todayDate.getFullYear() == year &&
			todayDate.getMonth() + 1 == month &&
			todayDate.getDate() == i + 1)
		{
			dayHTML = dayHTML.replace(color_1, color_3);
			dayHTML = dayHTML.replace(color_2, color_3);
			dayHTML = dayHTML.replace(color_4, color_3);
			dayHTML = dayHTML.replace("HIGHLIGHT_START", "<b>");
			dayHTML = dayHTML.replace("HIGHLIGHT_END", "</b>");
			calFocus = index;
			calFocus2 = (wwck);
		}

		dayHTML = dayHTML.replace("TITLE", "");	
		dayHTML = dayHTML.replace("COLOR", "");
		dayHTML = dayHTML.replace("HIGHLIGHT_START", "");
		dayHTML = dayHTML.replace("HIGHLIGHT_END", "");


		document.getElementById('dayCell' + index).innerHTML = dayHTML;


		/* lunar calnedar print */
/*		if (lunarDate.day == 1 || lunarDate.day == 15)
		{
			memoHTML = "<img src=/common/visualphone/images/" + (lunarDate.month < 10 ? "0" + lunarDate.month : lunarDate.month) + (lunarDate.day < 10 ? "0" + lunarDate.day: lunarDate.day) + ".gif border=0>";

		}
		else
			memoHTML = "<table border=0 cellpadding=0 cellspacing=0><tr height=8><td></td><tr></table>";

		document.getElementById('memoCell' + index).innerHTML = memoHTML;*/
	}
}

function lunarMonthCheck() { 
	if (document.getElementById('solarLunar').value == "solar") 
		document.getElementById('leapMonth').disabled = false;
	else
		document.getElementById('leapMonth').disabled = true;
}

var ayear = todayDate.getFullYear(), amonth = todayDate.getMonth() + 1;

function noAlpha(e) { 
    	if (e.keyCode < 48 || e.keyCode > 57)
	    e.returnValue = false;
}




//비주얼폰 달력에서 예약 년,월,일 설정
function calinput(aaa,bbb,ccc){
	var yy = document.calform.cal_year.value;
	var mm = document.calform.cal_month.value;
	var dd = aaa;
	document.Form1.yy.value = yy;
	if(mm>=10){
	document.Form1.mm.value = mm;
	}
	else{
	document.Form1.mm.value = "0"+mm;
	}
	if(dd>=10){
	document.Form1.dd.value = dd;
	}
	else{
	document.Form1.dd.value = "0"+dd;
	}

	var ccc1 = ""
	if (ccc==0) {
		ccc1 = color_1;
	} else if (ccc==6) {
		ccc1 = color_2;
	} else {
		ccc1 = color_4;
	}
	if (calFocus2==0) {
		ccc2 = color_1;
	} else if (calFocus2==6) {
		ccc2 = color_2;
	} else {
		ccc2 = color_4;
	}
	document.getElementById('dayCell' + calFocus).innerHTML = document.getElementById('dayCell' + calFocus).innerHTML.replace('<B>','').replace('</B>','').replace(color_3,ccc2);
	document.getElementById('dayCell' + bbb).innerHTML = document.getElementById('dayCell' + bbb).innerHTML.replace('<FONT ','<B><FONT ').replace('</FONT>','</FONT></B>').replace(ccc1,color_3);
	calFocus = bbb;
	calFocus2 = ccc;
	document.Form1.h.select();
}
