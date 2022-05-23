SELECT s.Id,
    s.Name,
	s.PrePrice,
	s.PreDeadline,
	rts.Id,
	rts.Name,
	rts.Price,
	rts.IsSpecificPrice,
    c.Id,
	c.ConditionText,
    cv.Id,
	cv.Value
FROM Services s
LEFT JOIN Rates rts
ON s.Id = rts.ServiceId
LEFT JOIN ConditionsValues cv
ON rts.Id = cv.RateId
LEFT JOIN Conditions c
ON c.Id = cv.ConditionId
WHERE s.Id = 2
ORDER BY rts.Price, c.Priority