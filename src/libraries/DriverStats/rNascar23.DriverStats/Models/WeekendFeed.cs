using System;
using System.Collections.Generic;
using System.Text;

namespace rNascar23.DriverStatistics.Models
{
    public class WeekendFeed
    {
        public Weekend_Race[] weekend_race { get; set; }
        public Weekend_Runs[] weekend_runs { get; set; }
    }

    public class Weekend_Race
    {
        public int race_id { get; set; }
        public int series_id { get; set; }
        public int race_season { get; set; }
        public string race_name { get; set; }
        public int race_type_id { get; set; }
        public bool restrictor_plate { get; set; }
        public int track_id { get; set; }
        public string track_name { get; set; }
        public DateTime date_scheduled { get; set; }
        public DateTime race_date { get; set; }
        public DateTime qualifying_date { get; set; }
        public DateTime tunein_date { get; set; }
        public float scheduled_distance { get; set; }
        public float actual_distance { get; set; }
        public int scheduled_laps { get; set; }
        public int actual_laps { get; set; }
        public int stage_1_laps { get; set; }
        public int stage_2_laps { get; set; }
        public int stage_3_laps { get; set; }
        public int stage_4_laps { get; set; }
        public int number_of_cars_in_field { get; set; }
        public int pole_winner_driver_id { get; set; }
        public float pole_winner_speed { get; set; }
        public int number_of_lead_changes { get; set; }
        public int number_of_leaders { get; set; }
        public int number_of_cautions { get; set; }
        public int number_of_caution_laps { get; set; }
        public float average_speed { get; set; }
        public string total_race_time { get; set; }
        public string margin_of_victory { get; set; }
        public int race_purse { get; set; }
        public string race_comments { get; set; }
        public int attendance { get; set; }
        public Result[] results { get; set; }
        public Caution_Segments[] caution_segments { get; set; }
        public Race_Leaders[] race_leaders { get; set; }
        public object[] infractions { get; set; }
        public Schedule[] schedule { get; set; }
        public Stage_Results[] stage_results { get; set; }
        public object[] pit_reports { get; set; }
        public string radio_broadcaster { get; set; }
        public string television_broadcaster { get; set; }
        public string satellite_radio_broadcaster { get; set; }
        public int master_race_id { get; set; }
        public bool inspection_complete { get; set; }
        public int playoff_round { get; set; }
    }

    public class Result
    {
        public int result_id { get; set; }
        public int finishing_position { get; set; }
        public int starting_position { get; set; }
        public string car_number { get; set; }
        public string driver_fullname { get; set; }
        public int driver_id { get; set; }
        public string driver_hometown { get; set; }
        public string hometown_city { get; set; }
        public string hometown_state { get; set; }
        public string hometown_country { get; set; }
        public int team_id { get; set; }
        public string team_name { get; set; }
        public int qualifying_order { get; set; }
        public int qualifying_position { get; set; }
        public float qualifying_speed { get; set; }
        public int laps_led { get; set; }
        public int times_led { get; set; }
        public string car_make { get; set; }
        public string car_model { get; set; }
        public string sponsor { get; set; }
        public int points_earned { get; set; }
        public int playoff_points_earned { get; set; }
        public int laps_completed { get; set; }
        public string finishing_status { get; set; }
        public int winnings { get; set; }
        public int series_id { get; set; }
        public int race_season { get; set; }
        public int race_id { get; set; }
        public string owner_fullname { get; set; }
        public int crew_chief_id { get; set; }
        public string crew_chief_fullname { get; set; }
        public int points_position { get; set; }
        public int points_delta { get; set; }
        public int owner_id { get; set; }
        public string official_car_number { get; set; }
        public bool disqualified { get; set; }
        public int diff_laps { get; set; }
        public int diff_time { get; set; }
    }

    public class Caution_Segments
    {
        public int race_id { get; set; }
        public int start_lap { get; set; }
        public int end_lap { get; set; }
        public string reason { get; set; }
        public string comment { get; set; }
        public string beneficiary_car_number { get; set; }
        public int flag_state { get; set; }
    }

    public class Race_Leaders
    {
        public int start_lap { get; set; }
        public int end_lap { get; set; }
        public string car_number { get; set; }
        public int race_id { get; set; }
    }

    public class Schedule
    {
        public string event_name { get; set; }
        public string notes { get; set; }
        public DateTime start_time_utc { get; set; }
        public int run_type { get; set; }
    }

    public class Stage_Results
    {
        public int stage_number { get; set; }
        public Result1[] results { get; set; }
    }

    public class Result1
    {
        public string driver_fullname { get; set; }
        public int driver_id { get; set; }
        public string car_number { get; set; }
        public int finishing_position { get; set; }
        public int stage_points { get; set; }
    }

    public class Weekend_Runs
    {
        public int weekend_run_id { get; set; }
        public int race_id { get; set; }
        public int timing_run_id { get; set; }
        public int run_type { get; set; }
        public string run_name { get; set; }
        public DateTime run_date { get; set; }
        public DateTime run_date_utc { get; set; }
        public Result2[] results { get; set; }
    }

    public class Result2
    {
        public int run_id { get; set; }
        public string car_number { get; set; }
        public string vehicle_number { get; set; }
        public string manufacturer { get; set; }
        public int driver_id { get; set; }
        public string driver_name { get; set; }
        public int finishing_position { get; set; }
        public float best_lap_time { get; set; }
        public float best_lap_speed { get; set; }
        public int best_lap_number { get; set; }
        public int laps_completed { get; set; }
        public string comment { get; set; }
        public float delta_leader { get; set; }
        public bool disqualified { get; set; }
    }
}
