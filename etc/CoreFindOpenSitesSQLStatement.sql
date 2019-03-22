


SELECT TOP 5 * FROM site
JOIN campground ON site.campground_id = campground.campground_id
WHERE (07 BETWEEN open_from_mm AND open_to_mm)
AND (07 BETWEEN open_from_mm AND open_to_mm)
AND (site.site_id IN (SELECT DISTINCT site.site_id FROM site LEFT JOIN reservation ON reservation.site_id = site.site_id
WHERE (reservation_id IS NULL AND site.campground_id = 1)
OR (site.campground_id = 1 AND NOT (('2019-02-20' <= reservation.to_date AND '2019-02-22' >= reservation.from_date)
OR (reservation.from_date <= '2019-02-20' AND reservation.to_date >= '2019-02-22')))));




SELECT * FROM reservation
JOIN site ON site.site_id = reservation.site_id
where campground_id = 1;