import { Helmet } from 'react-helmet-async';

import { CONFIG } from 'src/config-global';

import { TruckView } from 'src/sections/truck/view';

// ----------------------------------------------------------------------

export default function Page() {
  return (
    <>
      <Helmet>
        <title> {`Trucks - ${CONFIG.appName}`}</title>
      </Helmet>

      <TruckView />
    </>
  );
}
