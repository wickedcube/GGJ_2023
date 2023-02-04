<?php
$file = "leaderboard.json";

$input = file_get_contents('php://input');
$json = file_get_contents($file);
$data = json_decode($json, true);

$name = $input["username"];
$score = $input["highscore"];

$exists = false;
foreach ($data as &$record) {
    if ($record["username"] == $name) 
    {
        $record["username"] = $name;
        $record["highscore"] = $score;
        $exists = true;
        break;
    }
}

if (!$exists) {
    $data[] = array("username" => $name, "highscore" => $score);
}
$newJson = json_encode($data, JSON_PRETTY_PRINT);
file_put_contents($file, $newJson);

echo "got data". $name . "::" . $score; 
?>