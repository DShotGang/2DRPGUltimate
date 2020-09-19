<?php

function Install()
{
	include ('config.php');
		
	$error_msg = "";

	$db_error=false;
	$conn = new mysqli($dbhost, $dbuser, $dbpass, $dbname);

	// try to connect to the DB, if not display error
	if ($conn -> connect_error)
	{
	  $db_error=true;
	  $error_msg="Could not connect to MySQL database. Please check config.php. ".mysqli_error();
	}
	 
	if (!$db_error)
	{
		$db = mysqli_connect($dbhost, $dbuser,$dbpass);
		
		$q="CREATE TABLE `{$dbtable}` (
		`id` VARCHAR(255) NOT NULL,
		`version` VARCHAR(255) NOT NULL,
		`data` LONGTEXT NOT NULL, PRIMARY KEY (`id`)) ENGINE = InnoDB;"; 
		
		mysqli_select_db ($db, $dbname);
		if (!mysqli_query($db, $q))
		{
			$error_msg = "mysql querry error: ".mysqli_error($db);
		}
	}
	
	return $error_msg;
}

?>



<html>
<head>
<link href='https://fonts.googleapis.com/css?family=Barlow Condensed' rel='stylesheet'>
</head>
<style>
.tableshadow 
{ 
	box-shadow: 5px 10px 18px #000000; 
}
body {
    font-family: 'Barlow Condensed';font-size: 22px;
	background-color: #707070;
	background-image: linear-gradient(#707070, #2b2b2b); 
}
</style>
<body>
<table style="width: 100%; height: 100%; margin-left: auto; margin-right: auto; " border="0" cellspacing="0" cellpadding="0">
<tbody>
<tr>
<td>
<table class="tableshadow" style="width: 300px; height: 450px; background-color: #2b2b2b; margin-left: auto; margin-right: auto;" border="0" cellspacing="0" cellpadding="0">
<tbody>
<tr>
<td><img style="display: block; margin-left: auto; margin-right: auto;" src="logo.png" alt="" width="300" height="180" /></td>
</tr>
<tr>
<td style="text-align: center;">
<table style="width: 240px; margin-left: auto; margin-right: auto;" border="0" cellspacing="0" cellpadding="0">
<tbody>
<tr>
<td style="text-align: center; width: 240px;">
<span style="color: #f0f0f0;"><h3>Welcome to the DataBox cloud installation.</h3>
Please make sure you have configured the <strong>config.php</strong> file according to your MySQL database.

<br><br>
<?php
   if($_SERVER['REQUEST_METHOD']=='POST')
   {
	   $ok = Install();
	   
	   if (empty($ok))
	   {
		   echo "<br><span style='color:green;'>installation successful<br>You can now delete setup.php from your server.</span>";
		   ?><style type="text/css">
			form
			{
				display:none;
			}
			</style>
			<?php
	   }
	   else
	   {
		   echo "<br><span style='color:red;'>$ok</span>";
	   }
   } 
?>
</span>
</td>
</tr>
</tbody>
</table>
</td>
</tr>
<tr>
<td style="text-align: center;">
<div id ="form">
<form method="post" action="setup.php" >
    <input type="submit" value="INSTALL" name="submit" style="height:50px; width:200px"> <!-- assign a name for the button -->
</form>
</div>
</td>
</tr>
</tbody>
</table>
</td>
</tr>
</tbody>
</table>


</body>
</html>