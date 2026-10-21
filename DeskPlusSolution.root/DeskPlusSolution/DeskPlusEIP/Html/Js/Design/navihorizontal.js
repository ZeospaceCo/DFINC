/*!
  * navi
  * Copyright zeospace
  * Licensed zeospace
  */



$(function(){
	//console.log($.cookie('viewtype'));
	$('#mnav > li >  a').mouseover(function(e) {
		$(this).parent().addClass('over').siblings().removeClass('over');
		var index = $(this).parent('li').index();
		console.log(index);
		$.cookie('whatover',  index) ;
		$('.dropdown-menu').removeClass('show')
	});

	// portlet icon change by click
	$('.portlet-roll-icon').click(function() {
		$(this).children('i').toggleClass('fa-chevron-up');
	});

	if ( $("div").hasClass("indeximagecontrol ")  ){
		$( '.indeximagecontrol').mouseenter(function() {
			$(this).stop().animate({'right':'0px'},200);
			$(this).find('.modenavi > i.fa-chevron-left ').addClass('fa-chevron-right');
			console.log('on');
		}).mouseleave(function() {
			$( this).stop().animate({'right':'-250px'},200);
			$(this).find('.modenavi > i.fa-chevron-left ').removeClass('fa-chevron-right');
		});
		
		$( '.imgtoggle').click(function() {
			console.log('imgtoggle');
			$('body').toggleClass('imageoff');
			$(this).toggleClass('off');
		});
		
	}
	// icon view type 
});
$( document ).ready(function() {
	console.log($.cookie('whatover'));
	var index =  $.cookie('whatover');
	$('#mnav > li').eq(index).addClass('over');
});