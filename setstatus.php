<?php
$file = 'playstatus.txt';
if ($_GET["content"] != null)
{
	$current = $_GET["status"];
}
else
{
	$current = "nothing";
}
	file_put_contents($file, $current);
	echo("successfully set");
?>