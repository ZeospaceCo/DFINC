/*!
  * navi
  * Copyright zeospace
  * Licensed zeospace
  */

$(function(){
	//우상단 메뉴 감추기 아이콘
	$('.navi .viewtype').click(function(e) {
		e.preventDefault();
		$('body').toggleClass('navishow').toggleClass('navihide');
	});	
});	

jQuery(document).ready(function ($) {


if ($("body").hasClass("mobilehome")) {
	

	function resize() {
		var ww = $(window).width();
		var wh = $(window).height();	
		var mh = $('#mainpage').outerHeight(true)+35;
		if(ww > wh){  // landscape 
			console.log ('landscape');
			console.log(mh);
			$('.navi').css({'height': wh});
			$('.naviindex').css({'height': mh,});
		}else{ //portrait 
			console.log ('portrait');
			console.log(mh);
			$('.navi').removeAttr('style');
			//$('.naviindex').removeAttr('style');
			$('.naviindex').css({'height': mh, });
		}
		 
	};
	
	

	$(window).load(function () {
		resize()
	});

	$(window).resize(function(){
		resize()	
	});	
	

	var wh = $(window).height();	
	var mh = $('#mainpage').outerHeight(true);
	jQuery(window).scroll(function(){  
        if ($(this).scrollTop() > mh) {
			$('body').removeClass('scrollup');
			$('body').addClass('scrolldown');
  		}else{
			//console.log('scroll notover')
			$('body').removeClass('scrolldown');
			$('body').addClass('scrollup');
		};
  	});
	
	$('.navi').clone().addClass('naviindex').insertAfter('.navi').removeClass('navi');




}else{
	function resize() {
		var ww = $(window).width();
		var wh = $(window).height();	
		var mh = $('#mainpage').outerHeight(true);
		if(ww > wh){  // landscape 
			console.log(mh);
			console.log ('landscape');
			$('.navi').css('height', wh);

		}else{ //portrait 
			console.log ('portrait');
			console.log(mh);
			$('.navi').removeAttr('style');

		}
		
	};
	
	

	$(window).load(function () {
		resize()
        $(".mobile").css({ 'background-color': '#f9f9f9'    });
	});

	$(window).resize(function(){
		resize()	
	});	
		
	
}
		
});
