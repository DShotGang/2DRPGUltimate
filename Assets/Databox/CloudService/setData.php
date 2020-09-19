<?php
include ('config.php');


//$table = $_REQUEST["datatable"];
//$column = $_REQUEST["datacolumn"];
$id = $_REQUEST["id"];
$version = $_REQUEST["version"];
$data = $_REQUEST["data"];


$db = mysqli_connect($dbhost, $dbuser,$dbpass, $dbname);

$push = "INSERT INTO `{$dbtable}` (id, version, data) VALUES ('".$id."','".$version."','".$data."')
ON DUPLICATE KEY UPDATE id=$id, version='$version', data='$data'";

$pushdata = mysqli_query($db,$push);



if(! $pushdata )
{
  die('Could not enter data: ' . mysqli_error());
}
echo "success";

?>