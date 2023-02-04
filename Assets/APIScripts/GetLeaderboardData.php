<?php
header('Content-Type: application/json');
$jsonFile = file_get_contents("leaderboard.json");
echo $jsonFile;
?>