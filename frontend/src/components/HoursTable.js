import { React, useState } from "react";
import Paper from "@mui/material/Paper";
import Button from "@mui/material/Button";
import Calendar from "react-calendar";
import { Modal, TextField, Typography } from "@mui/material";
import Box from "@mui/material/Box";
import "react-calendar/dist/Calendar.css";

const style = {
  position: "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  width: 400,
  bgcolor: "background.paper",
  border: "2px solid #000",
  boxShadow: 24,
  p: 4,
};

const getLastFriday = () => {
  var d = new Date(),
    day = d.getDay();
  var diff = (7 - 5 + day) % 7;

  d.setDate(d.getDate() - diff);
  d.setHours(0);
  d.setMinutes(0);
  d.setSeconds(0);

  return d;
};

export const HoursTable = (props) => {
  const [modalOpen, setModalOpen] = useState(false);
  const [selectedDay, setSelectedDay] = useState(getLastFriday());

  console.log("Props in hours table", props);

  const handleCalendar = (date) => {
    console.log("Calendar:", date);

    if (date.getDay() === 5) {
      setSelectedDay(parseInt(date.valueOf()));
      setModalOpen(true);
    } else alert("Please select a Friday");
    // show modal
  };

  const handleClose = () => {
    console.log("closing modal:");
    setModalOpen(false);
  };

  const updateHours = (event) => {
    console.log("new hours:", event);
    var hoursParsed = parseInt(event);
    if (hoursParsed > -1) {
      props.updateHours(selectedDay, hoursParsed);
      handleClose();
    }
  };

  return (
    <Paper style={{ width: "70%", paddingTop: "10px" }}>
      <Calendar onClickDay={handleCalendar} />
      <Modal
        open={modalOpen}
        onClose={handleClose}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
      >
        <Box sx={style}>
          <Typography id="modal-modal-title" variant="h6" component="h2">
            Enter the number of hours for this week
          </Typography>
          <TextField
            onKeyPress={(e) => {
              if (e.key === "Enter") {
                updateHours(e.target.value);
              }
            }}
            defaultValue={props.getHoursForDate(selectedDay)}
            id="hours-entry"
            label="weekly hours"
            variant="outlined"
          />
        </Box>
      </Modal>
      <center style={{ padding: "10px" }}>
        <Button variant="contained" onClick={props.saveChanges}>
          Save Changes
        </Button>
      </center>
    </Paper>
  );
};
