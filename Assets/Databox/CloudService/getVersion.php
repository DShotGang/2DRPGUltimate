<?php
include ('config.php');


$id = $_REQUEST["id"];

$db = mysqli_connect($dbhost, $dbuser,$dbpass, $dbname);

$sql = "SELECT version FROM `{$dbtable}` WHERE id=$id";
$result = mysqli_query($db, $sql);

while($row = $result->fetch_assoc()) 
{
	echo $row["version"];
}
?>