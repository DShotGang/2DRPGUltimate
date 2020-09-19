<?php

date_default_timezone_set('Europe/London');

$timestamp = time();
$date_time = date("YmdHis", $timestamp); //("dmYHis", $timestamp);
echo $date_time;

?>