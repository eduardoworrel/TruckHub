import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import { Button, DialogActions } from '@mui/material';

export function SimpleDialog({ text, onClose }: { text: string; onClose: () => void }) {
  return (
    <Dialog onClose={onClose} open={text.length > 0}>
      <DialogTitle>Operação realizada com sucesso!</DialogTitle>
      <DialogContent>
        <DialogContentText id="alert-dialog-description">{text}</DialogContentText>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>OK</Button>
      </DialogActions>
    </Dialog>
  );
}
